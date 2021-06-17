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

        public CooperativesService(
            CheDbContext dbContext,
            IGradesService gradesService)
        {
            this._dbContext = dbContext;
            this._gradesService = gradesService;
        }

        public async Task<bool> CreateAsync(
            string name, 
            string info, 
            string gradeValue, 
            string creatorId,
            CooperativeAddressInputModel address)
        {
            //TODO: Get address from address service
            var cooperativeAddress = address.Map<CooperativeAddressInputModel, Address>();

            var cooperative = new Cooperative
            {
                Name = name,
                Info = info,
                CreatorId = creatorId,
                CreatedOn = DateTime.UtcNow,
                GradeId = await this._gradesService.GetGardeIdAsync(gradeValue),
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow },
                Address = cooperativeAddress
            };

            this._dbContext.Add(cooperative);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> UpdateAsync(
            string id, 
            string name, 
            string info, 
            string gradeValue,
            CooperativeAddressInputModel address)
        {
            var cooperativeToUpdate = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            cooperativeToUpdate.Name = name;
            cooperativeToUpdate.Info = info;
            cooperativeToUpdate.GradeId = await this._gradesService.GetGardeIdAsync(gradeValue);
            cooperativeToUpdate.ModifiedOn = DateTime.UtcNow;

            //TODO: Get address from address service
            var updatedAddress = address.Map<CooperativeAddressInputModel, Address>();
            cooperativeToUpdate.Address = updatedAddress;

            this._dbContext.Cooperatives.Update(cooperativeToUpdate);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var cooperativeToDelete = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            this._dbContext.Remove(cooperativeToDelete);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
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

        public async Task<IEnumerable<TEntity>> GetAllByCreatorAsync<TEntity>(string username) 
            => await this._dbContext.Cooperatives
                .AsNoTracking()
                .Where(x => x.Creator.UserName == username)
                .To<TEntity>()
                .ToListAsync();

        //TODO: Move members methods
        public async Task AddMemberAsync(string userId, string cooperativeId)
            => await this._dbContext.UserCooperatives.AddAsync(
                new CheUserCooperative
                {
                    CooperativeId = cooperativeId,
                    CheUserId = userId
                });

        public async Task RemoveMemberAsync(string memberId, string cooperativeId)
        {
            var cooperativeMember = await this._dbContext.UserCooperatives
                .SingleOrDefaultAsync(x => x.CheUserId == memberId && x.CooperativeId == cooperativeId);

            this._dbContext.UserCooperatives.Remove(cooperativeMember);

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetMembersAsync<TEntity>(string id)
            => await this._dbContext.UserCooperatives
                .AsNoTracking()
                .Where(x => x.CooperativeId == id)
                .To<TEntity>()
                .ToListAsync();

        public async Task<bool> CheckIfMemberAsync(string username, string cooperativeId)
            => await this._dbContext.UserCooperatives
                .AnyAsync(x => x.CheUser.UserName == username && x.CooperativeId == cooperativeId);

        public async Task<bool> CheckIfCreatorAsync(string username, string cooperativeId)
            => await this._dbContext.Cooperatives
                .AnyAsync(x => x.Creator.UserName == username && x.Id == cooperativeId);

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