namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;

    public class CooperativesService : ICooperativesService
    {
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IGradesService _gradesService;

        public CooperativesService(
            CheDbContext dbContext,
            IMapper mapper,
            IGradesService gradesService)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._gradesService = gradesService;
        }

        public async Task<bool> CreateAsync(string name, string info, string gradeValue, string creatorId)
        {
            var cooperativeNameExists = this._dbContext.Cooperatives.Any(x => x.Name == name);
            if (cooperativeNameExists)
            {
                return false;
            }

            var grade = await this._gradesService.GetByValueAsync(gradeValue);

            var cooperative = new Cooperative
            {
                Name = name,
                Info = info,
                CreatorId = creatorId,
                CreatedOn = DateTime.UtcNow,
                Grade = grade,
                Schedule = new Schedule { CreatedOn = DateTime.UtcNow }
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

            var updatedAddress = this._mapper.Map<TAddress, Address>(address);
            cooperativeToUpdate.Address = updatedAddress;


            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var cooperativeToDelete = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            cooperativeToDelete.IsDeleted = true;
            cooperativeToDelete.DeletedOn = DateTime.UtcNow;

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var cooperativeFromDb = await this._dbContext.Cooperatives
                .Where(x => x.Id == id)
                .Include(x => x.Members)
                .Include(x => x.JoinRequestsReceived)
                .ProjectTo<TEntity>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return cooperativeFromDb;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            var cooperativeFromDb = await this._dbContext
                .Cooperatives
                .Where(x => !x.IsDeleted)
                .ProjectTo<TEntity>(_mapper.ConfigurationProvider)
                .ToArrayAsync();

            return cooperativeFromDb;
        }

        public async Task<IEnumerable<TEntity>> GetCreatorAllByUsernameAsync<TEntity>(string username)
        {
            var cooperativeFromDb = await this._dbContext
                .Cooperatives
                .Where(x => x.Creator.UserName == username && !x.IsDeleted)
                .ProjectTo<TEntity>(_mapper.ConfigurationProvider)
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
    }
}