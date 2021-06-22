namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    using CHE.Data;
    using CHE.Services.Mapping;
    using CHE.Common;
    using CHE.Data.Models;

    public class CheUsersService : ICheUsersService
    {
        private readonly CheDbContext _dbContext;
        private readonly ICooperativesService _cooperativesService;

        public CheUsersService(CheDbContext dbContext, ICooperativesService cooperativesService)
        {
            this._dbContext = dbContext;
            this._cooperativesService = cooperativesService;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var teacher = await this._dbContext.Users
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return teacher;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1, 
            int endIndex = 0, 
            string schoolLevelFilter = null)
        {
            var count = endIndex == 0
                ? await this._dbContext.Users.CountAsync()
                : endIndex;

            var filteredTeachers = this.FilterCollection(schoolLevelFilter);

            var teachers = await filteredTeachers
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

            return teachers;
        }

        public async Task<int> Count(string schoolLevelFilter)
        {
            var filteredTachers = this.FilterCollection(schoolLevelFilter);

            return await filteredTachers.CountAsync();
        }

        public async Task AcceptRequestAsync(string requestId)
        {
            var request = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == requestId);

            
            // request is send from parent to cooperative
            if (request.ReceiverId == null)
            {
                await this._cooperativesService.AddMemberAsync(request.SenderId, request.CooperativeId);
            }
            // request is send from parent to teacher 
            else
            {
                await this._cooperativesService.AddMemberAsync(request.ReceiverId, request.CooperativeId);
            }

            this._dbContext.Remove(request);

            await this._dbContext.SaveChangesAsync();
        }

        public async Task RejectRequestAsync(string requestId)
        {
            var requestToDelete = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == requestId);

            this._dbContext.Remove(requestToDelete);

            await this._dbContext.SaveChangesAsync();
        }

        private IQueryable<CheUser> FilterCollection(string schoolLevelFilter = null)
        {
            var teachers = this._dbContext.Users
                .AsNoTracking()
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE);

            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                teachers = teachers.Where(x => x.Portfolio.SchoolLevel == schoolLevel);
            }

            return teachers;
        }
    }
}