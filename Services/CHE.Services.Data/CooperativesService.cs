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

        public async Task<bool> CreateAsync<TAddress>(string name, string info, string gradeValue, string creatorId, TAddress address)
        {
            //var cooperativeNameExists = this._dbContext.Cooperatives.Any(x => x.Name == name);
            //if (cooperativeNameExists)
            //{
            //    return false;
            //}

            var grade = await this._gradesService.GetByValueAsync(gradeValue);
            var cooperativeAddress = address.Map<TAddress, Address>();

            var cooperative = new Cooperative
            {
                Name = name,
                Info = info,
                CreatorId = creatorId,
                CreatedOn = DateTime.UtcNow,
                Grade = grade,
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow },
                Address = cooperativeAddress
            };

            await this._dbContext.AddAsync(cooperative);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> UpdateAsync<TAddress>(string id, string name, string info, string gradeValue, TAddress address)
        {
            var cooperativeToUpdate = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            cooperativeToUpdate.Name = name;
            cooperativeToUpdate.Info = info;
            cooperativeToUpdate.Grade = await this._gradesService.GetByValueAsync(gradeValue);
            cooperativeToUpdate.ModifiedOn = DateTime.UtcNow;

            var updatedAddress = address.Map<TAddress, Address>();
            cooperativeToUpdate.Address = updatedAddress;

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
        {
            //TODO get members and requests in different methods
            var cooperativeFromDb = await this._dbContext.Cooperatives
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return cooperativeFromDb;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1, 
            int endIndex = 0, 
            string filterGrade = null)
        {
            var count = endIndex == 0 
                ? await this._dbContext.Cooperatives.CountAsync() 
                : endIndex;

            var cooperatives = this._dbContext.Cooperatives.AsNoTracking();

            if (filterGrade != null)
            {
                cooperatives = cooperatives.Where(x => x.Grade.Value == filterGrade);
            }

            var filteredCooperatives = await cooperatives
                .Skip(startIndex - 1)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

            return filteredCooperatives;
        }

        public async Task<IEnumerable<TEntity>> GetCreatorAllByUsernameAsync<TEntity>(string username)
        {
            var cooperativeFromDb = await this._dbContext
                .Cooperatives
                .Where(x => x.Creator.UserName == username)
                .To<TEntity>()
                .ToArrayAsync();

            return cooperativeFromDb;
        }

        public async Task<bool> AddMemberAsync(string userId, string cooperativeId)
        {
            var memeber = new CheUserCooperative
            {
                CooperativeId = cooperativeId,
                CheUserId = userId
            };

            var memberAdded = await this._dbContext.UserCooperatives.AddAsync(memeber);

            var result = memberAdded.State == EntityState.Added;

            return result;
        }

        public async Task<bool> RemoveMemberAsync(string memberId, string cooperativeId)
        {
            var cooperativeMember = await this._dbContext.UserCooperatives
                .SingleOrDefaultAsync(x => x.CheUserId == memberId && x.CooperativeId == cooperativeId);

            this._dbContext.UserCooperatives.Remove(cooperativeMember);

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<IEnumerable<TEntity>> GetMembersAsync<TEntity>(string id)
        {
            var members = await this._dbContext.UserCooperatives
                .Where(x => x.CooperativeId == id)
                .To<TEntity>()
                .ToListAsync();

            return members;
        }

        public async Task<bool> CheckIfMemberAsync(string username, string cooperativeId)
        {
            var isMember = await this._dbContext.UserCooperatives
                .AnyAsync(x => x.CheUser.UserName == username && x.CooperativeId == cooperativeId);

            return isMember;
        }

        public async Task<bool> CheckIfCreatorAsync(string username, string cooperativeId)
        {
            var isCreator = await this._dbContext.Cooperatives
                .AnyAsync(x => x.Creator.UserName == username && x.Id == cooperativeId);

            return isCreator;
        }

        public async Task<IEnumerable<TEntity>> GetRequestsAsync<TEntity>(string id)
        {
            var requests = await this._dbContext.JoinRequests
                .Where(x => x.CooperativeId == id)
                .To<TEntity>()
                .ToListAsync();

            return requests;
        }

        public async Task<int> Count() =>
            await this._dbContext.Cooperatives
            .CountAsync();
    }
}