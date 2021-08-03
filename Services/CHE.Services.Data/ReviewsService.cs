namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

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

        public async Task<string> CreateAsync(
            string senderId, 
            string receiverId,
            string comment,
            int rating)
        {
            var review = new Review
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Comment = comment,
                Rating = rating,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            return review.Id;
        }

        public async Task UpdateAsync(string id, string comment, int rating)
        {
            var reviewToUpdate = await this._dbContext.Reviews
                .SingleOrDefaultAsync(x => x.Id == id);

            if (reviewToUpdate == null)
            {
                return;
            }

            reviewToUpdate.Comment = comment;
            reviewToUpdate.Rating = rating;
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
                .Where(x => x.ReceiverId == receiverId || x.Receiver.UserId == receiverId)
                .To<TEntity>()
                .ToListAsync();

        public async Task<string> GetSentReviewIdAsync(string userId, string receiverId) =>
            await this._dbContext.Reviews
                .AsNoTracking()
                .Where(x => x.Sender.UserId == userId && x.ReceiverId == receiverId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public async Task<bool> ExistsAsync(string senderId, string receiverId) =>
            await this._dbContext.Reviews
                .AnyAsync(x => x.SenderId == senderId && x.ReceiverId == receiverId);
    }
}