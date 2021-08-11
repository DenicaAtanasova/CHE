namespace CHE.Services.Data
{
    using CHE.Common.Extensions;
    using CHE.Data;
    using CHE.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class ParentsService : IParentsService
    {
        private readonly CheDbContext _dbContext;
        private readonly IJoinRequestsService _joinRequestsService;
        private readonly ICooperativesService _cooperativesService;
        private readonly IReviewsService _reviewsService;

        public ParentsService(
            CheDbContext dbContext, 
            IJoinRequestsService joinRequestsService,
            ICooperativesService cooperativesService,
            IReviewsService reviewsService)
        {
            this._dbContext = dbContext;
            this._joinRequestsService = joinRequestsService;
            this._cooperativesService = cooperativesService;
            this._reviewsService = reviewsService;
        }

        public async Task<string> CreateAsync(string userId)
        {
            var parent = new Parent
            {
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Parents.Add(parent);
            await this._dbContext.SaveChangesAsync();

            return parent.Id;
        }

        public async Task SendRequestAsync(string userId, string cooperativeId, string content)
        {
            var senderId = await this._dbContext.Parents
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (senderId == null)
            {
                return;
            }

            if (!await this._dbContext.Cooperatives
                .AnyAsync(x => x.Id == cooperativeId))
            {
                return;
            }

            if (await this._joinRequestsService
                .ExistsAsync(senderId, cooperativeId))
            {
                return;
            }

            await this._joinRequestsService
                    .CreateAsync(senderId, cooperativeId, content);
        }

        public async Task AcceptRequestAsync(string senderId, string cooperativeId, string requestId)
        {
            if (!await this._joinRequestsService
                .ExistsAsync(senderId, cooperativeId))
            {
                return;
            }

            await this._cooperativesService.AddMemberAsync(senderId, cooperativeId);

            await this._joinRequestsService.DeleteAsync(requestId);
        }

        public async Task RejectRequestAsync(string senderId, string cooperativeId, string requestId)
        {
            if (!await this._joinRequestsService
                .ExistsAsync(senderId, cooperativeId))
            {
                return;
            }

            await this._joinRequestsService.DeleteAsync(requestId);
        }

        public async Task SendReviewAsync(string userId, string receiverId, string comment, int rating)
        {
            var senderId = await this._dbContext.Parents
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            if (!senderId.IsValidString() || !receiverId.IsValidString())
            {
                return;
            }

            if (await this._dbContext.Teachers
                .AnyAsync(x => x.Id == receiverId))
            {
                return;
            }

            if (await this._reviewsService
                .ExistsAsync(senderId, receiverId))
            {
                return;
            }

            await this._reviewsService
                    .CreateAsync(senderId, receiverId, comment, rating);
        }
    }
}