namespace CHE.Services.Data
{
    using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<CheUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IGradesService _gradesService;

        public CooperativesService(
            CheDbContext dbContext, 
            UserManager<CheUser> userManager,
            IMapper mapper,
            IGradesService gradesService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._mapper = mapper;
            this._gradesService = gradesService;
        }

        #region Actions on cooperatives
        public async Task<bool> CreateAsync(string name, string info, string gradeValue, string creatorName)
        {
            var creator = await this._userManager.FindByNameAsync(creatorName);
            var grade = await this._gradesService.GetByValue(gradeValue);

            if (grade == null)
            {
                return false;
            }

            var cooperative = new Cooperative
            {
                Name = name,
                Info = info,
                Creator = creator,
                CreatedOn = DateTime.UtcNow,
                Grade = grade
            };

            await this._dbContext.AddAsync(cooperative);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> DeleteAsync(string? id)
        {
            var cooperativeToDelete = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            cooperativeToDelete.IsDeleted = true;
            cooperativeToDelete.ModifiedOn = DateTime.UtcNow;

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public Task UpdateAsync<TEntity>(string id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var cooperativeFromDb = await this._dbContext.Cooperatives
                .Where(x => x.Id == id)
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
                .ToListAsync();

            return cooperativeFromDb;
        }
        #endregion

        #region Actions with requests
        public Task AcceptRequest(string cooperativeId, string requestId)
        {
            throw new NotImplementedException();
        }

        public Task RejectRequest(string cooperativeId, string requestId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Actions with members
        public Task AddMemberAsync(string cooperativeId, string memberId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveMemberAsync(string cooperativeId, string memberId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Actions with teachers
        public Task SendTeacherRequest(string cooperativeId, string teacherId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveTeacherRequest(string cooperativeId, string teacherId)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}