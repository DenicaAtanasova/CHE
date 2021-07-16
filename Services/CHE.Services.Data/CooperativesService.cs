namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Cooperatives;

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
            CooperativeCreateInputModel inputModel)
        {
            var cooperative = new Cooperative
            {
                AdminId = adminId,
                Name = inputModel.Name,
                Info = inputModel.Info,
                CreatedOn = DateTime.UtcNow,
                GradeId = await this._gradesService.GetGardeIdAsync(inputModel.Grade),
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow },
                AddressId = await this._addressesService.GetAddressIdAsync(inputModel.Address)
            };

            this._dbContext.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            return cooperative.Id;
        }

        public async Task UpdateAsync(
            CooperativeUpdateInputModel inputModel)
        {
            var cooperativeToUpdate = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == inputModel.Id);

            if (cooperativeToUpdate == null)
            {
                return;
            }

            cooperativeToUpdate.Name = inputModel.Name;
            cooperativeToUpdate.Info = inputModel.Info;
            cooperativeToUpdate.GradeId = await this._gradesService.GetGardeIdAsync(inputModel.Grade);
            cooperativeToUpdate.ModifiedOn = DateTime.UtcNow;
            cooperativeToUpdate.AddressId = await this._addressesService.GetAddressIdAsync(inputModel.Address);

            this._dbContext.Cooperatives.Update(cooperativeToUpdate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var cooperativeFromDb = await this._dbContext.Cooperatives.SingleOrDefaultAsync(x => x.Id == id);

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
            var count = await this.GetCollectionCount(this._dbContext.Cooperatives, endIndex);

            var filteredCooperatives = this.GetFilteredCollection(gradeFilter, cityFilter, neighbourhoodFilter);

            return await this.GetCollectionPerPage<TEntity>(filteredCooperatives, startIndex, count);
        }

        public async Task<IEnumerable<TEntity>> GetAllByAdminAsync<TEntity>(
            string userId,
            int startIndex = 1,
            int endIndex = 0)
        {
            var cooperatives = this._dbContext.Cooperatives
                  .AsNoTracking()
                  .Where(x => x.AdminId == userId);

            var count = await this.GetCollectionCount(cooperatives, endIndex);

            return await this.GetCollectionPerPage<TEntity>(cooperatives, startIndex, count);
        }

        public async Task<IEnumerable<TEntity>> GetAllByAdminOrMemberAsync<TEntity>(
            string userId,
            int startIndex = 1,
            int endIndex = 0)
        {
            var cooperatives = this._dbContext.Cooperatives
                  .AsNoTracking()
                  .Where(x => x.AdminId == userId || x.Members.Any(x => x.CheUserId == userId));

            var count = await this.GetCollectionCount(cooperatives, endIndex);

            return await this.GetCollectionPerPage<TEntity>(cooperatives, startIndex, count);
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

        public async Task<IEnumerable<TEntity>> GetMembersAsync<TEntity>(string id) =>
            await this._dbContext.UserCooperatives
                .AsNoTracking()
                .Where(x => x.CooperativeId == id)
                .To<TEntity>()
                .ToListAsync();

        public async Task ChangeAdminAsync(string cooperativeId, string userId)
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

        public async Task<string> GetPendindRequestIdAsync(string cooperativeId, string senderId) =>
            await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.SenderId == senderId && x.CooperativeId == cooperativeId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public async Task<int> CountAsync(string userId) =>
            await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.AdminId == userId)
                .CountAsync();

        public async Task<int> CountAsync(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null) => 
                await this.GetFilteredCollection(gradeFilter, cityFilter, neighbourhoodFilter).CountAsync();

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

        private async Task<int> GetCollectionCount(IQueryable<Cooperative> cooperatives, int endIndex)
            => endIndex == 0
                ? await cooperatives.CountAsync()
                : endIndex;

        private async Task<IEnumerable<TEntity>> GetCollectionPerPage<TEntity>(
            IQueryable<Cooperative> cooperatives, 
            int startIndex, 
            int count) =>
            await cooperatives
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();
    }
}