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
            var grade = await this._gradesService.GetByValueAsync(gradeValue);

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

        public async Task<bool> UpdateAsync(string id, string name, string info, string gradeValue, string city, string neighbourhood, string street = null)
        {
            var cooperativeToUpdate = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            cooperativeToUpdate.Name = name;
            cooperativeToUpdate.Info = info;
            cooperativeToUpdate.Grade = await this._gradesService.GetByValueAsync(gradeValue);

            var address = new Address
            {
                City = city,
                Neighbourhood = neighbourhood,
                Street = street,
                Cooperative = cooperativeToUpdate
            };
            await this._dbContext.Addresses.AddAsync(address);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var cooperativeToDelete = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == id);

            cooperativeToDelete.IsDeleted = true;
            cooperativeToDelete.ModifiedOn = DateTime.UtcNow;

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var cooperativeFromDb = await this._dbContext.Cooperatives
                .Where(x => x.Id == id)
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
                .ToListAsync();

            return cooperativeFromDb;
        }
        #endregion

        #region Actions with members
        public async Task<bool> AddMemberAsync(string cooperativeId, string memberUsername)
        {
            var member = await this._userManager.FindByNameAsync(memberUsername);
            member.Cooperatives.Add(
                new CheUserCooperative
                {
                    CooperativeId = cooperativeId
                }
            );

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public Task RemoveMemberAsync(string cooperativeId, string memberId)
        {
            throw new NotImplementedException();
        }
        #endregion

        //TODO: Move to users service
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