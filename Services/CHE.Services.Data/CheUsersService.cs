namespace CHE.Services.Data
{
    using CHE.Common;
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

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
        private readonly IReviewsService _reviewsService;

        public CheUsersService(
            CheDbContext dbContext,
            ICooperativesService cooperativesService,
            IJoinRequestsService joinRequestsService,
            IReviewsService reviewsService)
        {
            this._dbContext = dbContext;
            this._cooperativesService = cooperativesService;
            this._joinRequestsService = joinRequestsService;
            this._reviewsService = reviewsService;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
            => await this._dbContext.Users
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            string role,
            int startIndex = 1,
            int endIndex = 0,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var filteredUsers = this.GetFilteredCollection(
                role, schoolLevelFilter, cityFilter, neighbourhoodFilter);

            var count = endIndex == 0
                ? await filteredUsers.CountAsync()
                : endIndex;

            return await filteredUsers
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();
        }

        public async Task<int> CountAsync(
            string role,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var filteredUsers = this.GetFilteredCollection(role, schoolLevelFilter, cityFilter, neighbourhoodFilter);

            return await filteredUsers.CountAsync();
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

        public async Task SendRequestAsync(
            string senderId,
            string content,
            string cooperativeId,
            string receiverId)
        {
            var cooperativeExists = await this._dbContext.Cooperatives
                .AnyAsync(x => x.Id == cooperativeId);

            if (!cooperativeExists)
            {
                return;
            }

            var requestExists = await this._dbContext.JoinRequests
                .AnyAsync(x => x.CooperativeId == cooperativeId &&
                               x.ReceiverId == receiverId &&
                               x.SenderId == senderId);

            if (!requestExists)
            {
                await this._joinRequestsService.CreateAsync(senderId, content, cooperativeId, receiverId);
            }
        }

        public async Task SendReviewAsync(
            string senderId,
            string receiverId,
            string comment,
            int rating)
        {
            var reviewExists = await this._dbContext.Reviews
                .AnyAsync(x => x.SenderId == senderId && x.ReceiverId == receiverId);

            if (!reviewExists)
            {
                await this._reviewsService
                    .CreateAsync(senderId, receiverId, comment, rating);
            }
        }

        private IQueryable<CheUser> GetFilteredCollection(
            string role,
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var users = GetUsersInRole(role);

            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                return users.Where(x => x.Profile.SchoolLevel == schoolLevel);
            }

            if (cityFilter != null)
            {
                users = users.Where(x => x.Profile.Address.City == cityFilter);
            }

            if (neighbourhoodFilter != null)
            {
                users = users.Where(x => x.Profile.Address.Neighbourhood == neighbourhoodFilter);
            }

            return users;
        }

        private IQueryable<CheUser> GetUsersInRole(string roleName) =>
            this._dbContext.Users
                .Join(this._dbContext.UserRoles,
                    u => u.Id,
                    ur => ur.UserId,
                    (u, ur) => new
                    {
                        User = u,
                        UserRole = ur
                    })
                .Join(this._dbContext.Roles,
                    u => u.UserRole.RoleId,
                    r => r.Id,
                    (u, r) => new
                    {
                        User = u.User,
                        Role = r
                    })
                .Where(u => u.Role.Name == roleName)
                .Select(u => u.User);
    }
}