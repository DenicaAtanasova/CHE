namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Reviews;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewsService : IReviewsService
    {
        private readonly CheDbContext _dbContext;

        public ReviewsService(
            CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<string> CreateAsync(string senderId, ReviewCreateInputModel inputModel)
        {
            var review = inputModel.Map<ReviewCreateInputModel, Review>();
            review.CreatedOn = DateTime.UtcNow;
            review.SenderId = senderId;

            await this._dbContext.Reviews.AddAsync(review);
            await this._dbContext.SaveChangesAsync();

            return review.Id;
        }

        public async Task<IEnumerable<TEntity>> GetAllByReceiverAsync<TEntity>(string receiverId)
            => await this._dbContext.Reviews
                .AsNoTracking()
                .Where(x => x.ReceiverId == receiverId)
                .To<TEntity>()
                .ToListAsync();
    }
}