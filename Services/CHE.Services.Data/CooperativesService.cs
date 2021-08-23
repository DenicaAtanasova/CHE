namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Data.Models.Enums;
    using CHE.Services.Data.Enums;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CooperativesService : ICooperativesService
    {
        private readonly CheDbContext _dbContext;
        private readonly IAddressesService _addressesService;
        private readonly IMessengersService _messengersService;

        public CooperativesService(
            CheDbContext dbContext,
            IAddressesService addressesService,
            IMessengersService messengersService)
        {
            this._dbContext = dbContext;
            this._addressesService = addressesService;
            this._messengersService = messengersService;
        }

        public async Task<string> CreateAsync(
            string userId,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood)
        {
            var adminId = await this._dbContext.Parents
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var cooperative = new Cooperative
            {
                AdminId = adminId,
                Name = name,
                Info = info,
                Grade = Enum.Parse<Grade>(grade),
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow },
                Messenger = new Messenger 
                { 
                    CreatedOn = DateTime.UtcNow,
                    Users = new List<MessengerUser>
                    {
                        new MessengerUser 
                        {
                            UserId = userId 
                        }
                    }
                },
                AddressId = await this._addressesService
                    .GetAddressIdAsync(city, neighbourhood),
                CreatedOn = DateTime.UtcNow,
            };

            this._dbContext.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            return cooperative.Id;
        }

        public async Task UpdateAsync(
            string id,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood)
        {
            var cooperativeToUpdate = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            if (cooperativeToUpdate == null)
            {
                return;
            }

            cooperativeToUpdate.Name = name;
            cooperativeToUpdate.Info = info;
            cooperativeToUpdate.Grade = Enum.Parse<Grade>(grade);
            cooperativeToUpdate.AddressId = await this._addressesService
                .GetAddressIdAsync(city, neighbourhood);
            cooperativeToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Cooperatives.Update(cooperativeToUpdate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cooperativeFromDb = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            if (cooperativeFromDb == null)
            {
                return;
            }

            this._dbContext.Remove(cooperativeFromDb);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id) =>
            await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1, 
            int endIndex = 0, 
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var filteredCooperatives = this.GetFilteredCollection(gradeFilter, cityFilter, neighbourhoodFilter);

            var count = await this.GetCollectionCountAsync(filteredCooperatives, endIndex);

            return await this.GetCollectionPerPageAsync<TEntity>(filteredCooperatives, startIndex, count);
        }

        public async Task<IEnumerable<TEntity>> GetAllByUserAsync<TEntity>(
            string userId,
            CooperativeUserType userType) =>
            await this.GetCollectionByUser(userId, userType)
                .To<TEntity>()
                .ToListAsync();

        public async Task AddMemberAsync(string parentId, string cooperativeId)
        {
            this._dbContext.ParentsCooperatives.Add(
                new ParentCooperative
                {
                    CooperativeId = cooperativeId,
                    ParentId = parentId
                });

            var messengerId = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            var userId = await this._dbContext.Parents
                .Where(x => x.Id == parentId)
                .Select(x => x.UserId)
                .SingleOrDefaultAsync();

            await this._messengersService.AddMemberAsync(messengerId, userId);

            await this._dbContext.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(string userId, string cooperativeId)
        {
            var member = await this._dbContext.ParentsCooperatives
                .SingleOrDefaultAsync(x => x.Parent.UserId == userId && 
                                           x.CooperativeId == cooperativeId);

            if (member == null)
            {
                return;
            }

            this._dbContext.ParentsCooperatives.Remove(member);

            var messengerId = await this._dbContext.Messengers
                .Where(x => x.CooperativeId == cooperativeId)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            await this._messengersService.RemoveMemberAsync(messengerId, userId);

            await this._dbContext.SaveChangesAsync();
        }

        public async Task ChangeAdminAsync(string userId, string cooperativeId)
        {
            var cooperative = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == cooperativeId);

            if (cooperative == null)
            {
                return;
            }

            var member = this._dbContext.ParentsCooperatives
                .SingleOrDefault(x => x.ParentId == userId &&
                                      x.CooperativeId == cooperativeId);

            if (member == null)
            {
                return;
            }

            this._dbContext.ParentsCooperatives.Remove(member);

            this._dbContext.ParentsCooperatives.Add(
                new ParentCooperative
                {
                    CooperativeId = cooperativeId,
                    ParentId = cooperative.AdminId
                });

            cooperative.AdminId = userId;

            this._dbContext.Cooperatives.Update(cooperative);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfAdminAsync(string userId, string cooperativeId) =>
            await this._dbContext.Cooperatives
                .AnyAsync(x => x.Admin.UserId == userId &&
                               x.Id == cooperativeId);

        public async Task<bool> CheckIfMemberAsync(string userId, string cooperativeId) =>
            await this._dbContext.ParentsCooperatives
                .AnyAsync(x => x.Parent.UserId == userId && x.CooperativeId == cooperativeId);

        public async Task<int> CountAsync(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null) => 
                await this.GetFilteredCollection(gradeFilter, cityFilter, neighbourhoodFilter)
                    .CountAsync();

        private IQueryable<Cooperative> GetFilteredCollection(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var cooperatives = this._dbContext.Cooperatives.AsNoTracking();

            if (gradeFilter != null)
            {
                var grade = Enum.Parse<Grade>(gradeFilter);
                cooperatives = cooperatives.Where(x => x.Grade == grade);
            }

            if (cityFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.City == cityFilter);
            }

            if (neighbourhoodFilter != null)
            {
                cooperatives = cooperatives.Where(x => x.Address.Neighbourhood == neighbourhoodFilter);
            }

            return cooperatives;
        }

        private async Task<int> GetCollectionCountAsync(IQueryable<Cooperative> cooperatives, int endIndex)
            => endIndex == 0
                ? await cooperatives.CountAsync()
                : endIndex;

        private async Task<IEnumerable<TEntity>> GetCollectionPerPageAsync<TEntity>(
            IQueryable<Cooperative> cooperatives, 
            int startIndex, 
            int count) =>
            await cooperatives
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

        private IQueryable<Cooperative> GetCollectionByUser(string userId, CooperativeUserType participant) =>
            participant switch
            {
                CooperativeUserType.Admin => 
                    _dbContext.Cooperatives
                        .AsNoTracking()
                        .Where(x => x.Admin.UserId == userId),
                CooperativeUserType.Member | CooperativeUserType.Admin => 
                    _dbContext.Cooperatives
                        .AsNoTracking()
                        .Where(x => x.Admin.UserId == userId || 
                                    x.Members.Any(x => x.Parent.UserId == userId)),
                CooperativeUserType.Other => _dbContext.Cooperatives.AsNoTracking(),
                _ => null
            };
    }
}