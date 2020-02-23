namespace CHE.Services.Data
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CHE.Data;
    using CHE.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

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
        public async Task CreateAsync(string name, string info, string gradeValue, string creatorName)
        {
            var creator = await this._userManager.FindByNameAsync(creatorName);
            var grade = await this._gradesService.GetByValue(gradeValue);
            //TODO: Validate
            if (grade == null)
            {

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
            await this._dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync<TEntity>(string id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            var cooperativeFromDb = await this._dbContext
                .Cooperatives
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