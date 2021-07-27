namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
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
        private readonly IGradesService _gradesService;
        private readonly IAddressesService _addressesService;

        public CooperativesService(
            CheDbContext dbContext,
            IGradesService gradesService,
            IAddressesService addressesService)
        {
            this._dbContext = dbContext;
            this._gradesService = gradesService;
            this._addressesService = addressesService;
        }

        public async Task<string> CreateAsync(
            string adminId,
            string name,
            string info,
            string grade,
            string city,
            string neighbourhood)
        {
            var cooperative = new Cooperative
            {
                AdminId = adminId,
                Name = name,
                Info = info,
                GradeId = await this._gradesService.GetGardeIdAsync(grade),
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow },
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
            cooperativeToUpdate.GradeId = await this._gradesService.GetGardeIdAsync(grade);
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
            CooperativeUserType userType,
            int startIndex = 1,
            int endIndex = 0)
        {
            var userCoperatives = this.GetCollectionByUser(userId, userType);

            var count = await this.GetCollectionCountAsync(userCoperatives, endIndex);

            return await this.GetCollectionPerPageAsync<TEntity>(userCoperatives, startIndex, count);
        }

        public async Task AddMemberAsync(string userId, string cooperativeId)
        {
            this._dbContext.UserCooperatives.Add(
                new CheUserCooperative
                {
                    CooperativeId = cooperativeId,
                    CheUserId = userId
                });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task RemoveMemberAsync(string memberId, string cooperativeId)
        {
            var member = await this._dbContext.UserCooperatives
                .SingleOrDefaultAsync(x => x.CheUserId == memberId && x.CooperativeId == cooperativeId);

            if (member == null)
            {
                return;
            }

            this._dbContext.UserCooperatives.Remove(member);
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

            var newAdmin = cooperative.Members
                .SingleOrDefault(x => x.CheUserId == userId);

            if (newAdmin == null)
            {
                return;
            }

            this._dbContext.UserCooperatives.Remove(newAdmin);

            this._dbContext.UserCooperatives.Add(
                new CheUserCooperative
                {
                    CooperativeId = cooperativeId,
                    CheUserId = cooperative.AdminId
                });

            cooperative.AdminId = userId;

            this._dbContext.Cooperatives.Update(cooperative);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<bool> CheckIfAdminAsync(string userId, string cooperativeId) =>
            await this._dbContext.Cooperatives
                .AnyAsync(x => x.AdminId == userId && x.Id == cooperativeId);

        public async Task<bool> CheckIfMemberAsync(string userId, string cooperativeId) =>
            await this._dbContext.UserCooperatives
                .AnyAsync(x => x.CheUserId == userId && x.CooperativeId == cooperativeId);

        public async Task<int> CountByUserAsync(string userId) =>
            await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.AdminId == userId || x.Members.Any(x => x.CheUserId == userId))
                .CountAsync();

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
                cooperatives = cooperatives.Where(x => x.Grade.Value == gradeFilter);
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
                        .Where(x => x.AdminId == userId),
                CooperativeUserType.Member | CooperativeUserType.Admin => 
                    _dbContext.Cooperatives
                        .AsNoTracking()
                        .Where(x => x.AdminId == userId || x.Members.Any(x => x.CheUserId == userId)),
                CooperativeUserType.Other => _dbContext.Cooperatives.AsNoTracking(),
                _ => null
            };
    }
}