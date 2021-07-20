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

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            return review.Id;
        }
        public async Task UpdateAsync(ReviewUpdateInputModel inputModel)
        {
            var reviewToUpdate = await this._dbContext.Reviews
                .SingleOrDefaultAsync(x => x.Id == inputModel.Id);

            if (reviewToUpdate == null)
            {
                return;
            }

            reviewToUpdate.Comment = inputModel.Comment;
            reviewToUpdate.Rating = inputModel.Rating;
            reviewToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Reviews.Update(reviewToUpdate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var review = await this._dbContext.Reviews
                .SingleOrDefaultAsync(x => x.Id == id);

            if (review == null)
            {
                return;
            }

            this._dbContext.Reviews.Remove(review);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id) =>
            await this._dbContext.Reviews
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllByReceiverAsync<TEntity>(string receiverId)
            => await this._dbContext.Reviews
                .AsNoTracking()
                .Where(x => x.ReceiverId == receiverId)
                .To<TEntity>()
                .ToListAsync();

        public async Task<string> GetSentReviewIdAsync(string receiverId, string senderId) =>
            await this._dbContext.Reviews
                .AsNoTracking()
                .Where(x => x.SenderId == senderId && x.ReceiverId == receiverId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
    }
}