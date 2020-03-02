namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CHE.Data;
    using CHE.Data.Models;

    public class ReviewsService : IReviewsService
    {
        private readonly CheDbContext _dbContext;

        public ReviewsService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(string comment, int rating, string senderId, string receiverId)
        {
            var review = new Review
            {
                Comment = comment,
                Rating = rating,
                SenderId = senderId,
                ReceiverId = receiverId,
                CreatedOn = DateTime.UtcNow
            };

            await this._dbContext.Reviews.AddAsync(review);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }


        public Task<IEnumerable<TEntity>> GetTeachersAllAsync<TEntity>(string teacherId)
        {
            throw new System.NotImplementedException();
        }
    }
}