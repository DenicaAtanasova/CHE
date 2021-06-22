namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Cooperatives;

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
            string creatorId,
            CooperativeCreateInputModel inputModel)
        {
            var cooperative = new Cooperative
            {
                CreatorId = creatorId,
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
            var cooperativeToUpdate = new Cooperative { Id = inputModel.Id };
            cooperativeToUpdate.Name = inputModel.Name;
            cooperativeToUpdate.Info = inputModel.Info;
            cooperativeToUpdate.CreatorId = inputModel.CreatorId;
            cooperativeToUpdate.CreatedOn = inputModel.CreatedOn;
            cooperativeToUpdate.GradeId = await this._gradesService.GetGardeIdAsync(inputModel.Grade);
            cooperativeToUpdate.ModifiedOn = DateTime.UtcNow;
            cooperativeToUpdate.AddressId = await this._addressesService.GetAddressIdAsync(inputModel.Address);

            this._dbContext.Cooperatives.Update(cooperativeToUpdate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            this._dbContext.Remove(new Cooperative { Id = id });
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
            => await this._dbContext.Cooperatives
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
            var count = endIndex == 0 
                ? await this._dbContext.Cooperatives.CountAsync() 
                : endIndex;

            var filteredCooperatives = this.FilterCollection(gradeFilter, cityFilter, neighbourhoodFilter);

            var cooperatives = await filteredCooperatives
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

            return cooperatives;
        }

        public async Task<IEnumerable<TEntity>> GetAllByCreatorAsync<TEntity>(string userId) 
            => await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.Creator.Id == userId)
                .To<TEntity>()
                .ToListAsync();

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
            this._dbContext.UserCooperatives.Remove(
                new CheUserCooperative
                {
                    CooperativeId = cooperativeId,
                    CheUserId = memberId
                });

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetMembersAsync<TEntity>(string id)
            => await this._dbContext.UserCooperatives
                .AsNoTracking()
                .Where(x => x.CooperativeId == id)
                .To<TEntity>()
                .ToListAsync();

        public async Task<bool> CheckIfMemberAsync(string userId, string cooperativeId)
            => await this._dbContext.UserCooperatives
                .AnyAsync(x => x.CheUserId == userId && x.CooperativeId == cooperativeId);

        public async Task<bool> CheckIfCreatorAsync(string userId, string cooperativeId)
            => await this._dbContext.Cooperatives
                .AnyAsync(x => x.Creator.Id == userId && x.Id == cooperativeId);

        public async Task<int> Count(
            string gradeFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
            => await this.FilterCollection(gradeFilter, cityFilter, neighbourhoodFilter).CountAsync();

        private IQueryable<Cooperative> FilterCollection(
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
    }
}