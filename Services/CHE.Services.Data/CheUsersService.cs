namespace CHE.Services.Data
{ 
    using CHE.Common;
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.JoinRequests;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CheUsersService : ICheUsersService
    {
        private readonly CheDbContext _dbContext;
        private readonly ICooperativesService _cooperativesService;
        private readonly IJoinRequestsService _joinRequestsService;

        public CheUsersService(
            CheDbContext dbContext, 
            ICooperativesService cooperativesService,
            IJoinRequestsService joinRequestsService)
        {
            this._dbContext = dbContext;
            this._cooperativesService = cooperativesService;
            this._joinRequestsService = joinRequestsService;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
            => await this._dbContext.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1, 
            int endIndex = 0, 
            string schoolLevelFilter = null)
        {
            var count = endIndex == 0
                ? await this._dbContext.Users
                    .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE)
                    .CountAsync()
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

        public async Task SendRequestAsync(string senderId, JoinRequestInputModel inputModel)
        {
            var requestId = await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.CooperativeId == inputModel.CooperativeId &&
                            x.ReceiverId == inputModel.ReceiverId &&
                            x.SenderId == senderId)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            if (requestId is null)
            {
                await this._joinRequestsService.CreateAsync(
                    inputModel.Content, 
                    inputModel.CooperativeId, 
                    senderId, 
                    inputModel.ReceiverId);
            }
        }

        private IQueryable<CheUser> FilterCollection(string schoolLevelFilter = null)
        {
            var teachers = this._dbContext.Users
                .AsNoTracking()
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE);

            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                return teachers.Where(x => x.Portfolio.SchoolLevel == schoolLevel);
            }

            return teachers;
        }
    }
}