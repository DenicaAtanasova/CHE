namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;

    public class ReviewsService : IReviewsService
    {
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReviewsService(
            CheDbContext dbContext,
            IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<bool> CreateAsync(string comment, int rating, string senderId, string receiverId)
        {
            var reviewExisits = await this._dbContext.Reviews
                .AnyAsync(x => x.ReceiverId == receiverId && x.SenderId == senderId);
            if (reviewExisits)
            {
                return false;
            }

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


        public async Task<IEnumerable<TEntity>> GetTeachersAllAsync<TEntity>(string teacherId)
        {
            var reviews = await this._dbContext.Reviews
                .Where(x => x.ReceiverId == teacherId && !x.IsDeleted)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .ToArrayAsync();

            return reviews;
        }
    }
}