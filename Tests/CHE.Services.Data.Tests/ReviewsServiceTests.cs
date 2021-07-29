namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Reviews;
    using CHE.Web.ViewModels.Reviews;
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class ReviewsServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IReviewsService _ReviewsService;

        public ReviewsServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            this._ReviewsService = new ReviewsService(this._dbContext);

            AutoMapperConfig.RegisterMappings(
                typeof(ReviewCreateInputModel).Assembly,
                typeof(ReviewAllViewModel).Assembly);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewReview()
        {
            var senderId = Guid.NewGuid().ToString();
            var receiverId = Guid.NewGuid().ToString();
            var comment = "Comment";
            var rating = 5;

            var reviewId = await this._ReviewsService.CreateAsync(senderId, receiverId, comment, rating);
            var reviewFromDb = await this._dbContext.Reviews.SingleOrDefaultAsync();

            Assert.Equal(reviewId, reviewFromDb.Id);
            Assert.Equal(senderId, reviewFromDb.SenderId);
            Assert.Equal(receiverId, reviewFromDb.ReceiverId);
            Assert.Equal(comment, reviewFromDb.Comment);
            Assert.Equal(rating, reviewFromDb.Rating);

            var expectedCreatedOn = DateTime.UtcNow;
            Assert.Equal(expectedCreatedOn, reviewFromDb.CreatedOn, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateReview()
        {
            var senderId = Guid.NewGuid().ToString();
            var receiverId = Guid.NewGuid().ToString();

            var review = new Review
            {
                Comment = "Comment",
                Rating = 2,
                SenderId = senderId, 
                ReceiverId = receiverId
            };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            var comment = "Updated Comment";
            var rating = 5;

            await this._ReviewsService.UpdateAsync(review.Id, comment, rating);
            var reviewFromDb = await this._dbContext.Reviews.SingleOrDefaultAsync();
            var ModifiedOn = DateTime.UtcNow;

            Assert.Equal(comment, reviewFromDb.Comment);
            Assert.Equal(rating, reviewFromDb.Rating);

            Assert.Equal(ModifiedOn, reviewFromDb.ModifiedOn.Value, 
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteReview()
        {
            var review = new Review
            {
                Comment = "Comment",
                Rating = 2,
                SenderId = Guid.NewGuid().ToString(),
                ReceiverId = Guid.NewGuid().ToString()
        };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            await this._ReviewsService.DeleteAsync(review.Id);

            var reviewFromDb = await this._dbContext.Reviews.SingleOrDefaultAsync();

            Assert.Null( reviewFromDb);
        }

        [Fact]
        public async Task GetAllByReceiverAsync_ShouldReturnAllReviewsByReceiver()
        {
            var searchedReceiver = new CheUser
            {
                UserName = "Maria"
            };

            var reviewsList = new List<Review>
            {
                new Review
                {
                    Comment = "Comment1",
                    Receiver = searchedReceiver
                },
                new Review
                {
                    Comment = "Comment2",
                    Receiver = searchedReceiver
                },
                new Review
                {
                    Comment = "Comment3",
                    ReceiverId = Guid.NewGuid().ToString()
                },
                new Review
                {
                    Comment = "Comment4",
                    ReceiverId = Guid.NewGuid().ToString()
                }
            };

            this._dbContext.Reviews.AddRange(reviewsList);
            await this._dbContext.SaveChangesAsync();

            var receiverReviews = await this._ReviewsService
                .GetAllByReceiverAsync<ReviewAllViewModel>(searchedReceiver.Id);
            var expectedReviewsList = reviewsList.Where(x => x.ReceiverId == searchedReceiver.Id).ToList();

            Assert.Equal(expectedReviewsList.Count(), receiverReviews.Count());

            var index = 0;
            foreach (var review in receiverReviews)
            {
                Assert.Equal(expectedReviewsList[index++].Comment, review.Comment);
            }
        }

        [Fact]
        public async Task GetByIdAsync_Review_ShouldReturnCorrectReview()
        {
            var senderId = Guid.NewGuid().ToString();
            var receiverId = Guid.NewGuid().ToString();

            var review = new Review
            {
                Comment = "Comment",
                Rating = 2,
                SenderId = Guid.NewGuid().ToString(),
                ReceiverId = Guid.NewGuid().ToString()
            };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            var reviewFromDb = await this._ReviewsService
                .GetByIdAsync<ReviewUpdateInputModel>(review.Id);

            Assert.Equal(review.Id, reviewFromDb.Id);
        }

        [Fact]
        public async Task GetSentReviewIdAsync_ShouldReturnCorrectReview()
        {
            var review = new Review
            {
                Comment = "Comment",
                Rating = 2,
                Sender = new CheUser 
                { 
                    UserName = "Sender"
                },
                Receiver = new CheUser
                {
                    UserName = "Receiver"
                }
            };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            var reviewId = await this._ReviewsService
                .GetSentReviewIdAsync(review.SenderId, review.ReceiverId);

            Assert.Equal(review.Id, reviewId);
        }

        [Fact]
        public async Task ExistsAsync_WhenReviewExists_ShouldReturnTrue()
        {
            var review = new Review
            {
                Comment = "Comment",
                Rating = 2,
                Sender = new CheUser
                {
                    UserName = "Sender"
                },
                Receiver = new CheUser
                {
                    UserName = "Receiver"
                }
            };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            Assert.True(await this._ReviewsService
                .ExistsAsync(review.SenderId, review.ReceiverId));
        }

        [Fact]
        public async Task ExistsAsync_WhenReviewDoesNotExists_ShouldReturnFalse()
        {
            var review = new Review
            {
                Comment = "Comment",
                Rating = 2,
                Sender = new CheUser
                {
                    UserName = "Sender"
                },
                Receiver = new CheUser
                {
                    UserName = "Receiver"
                }
            };

            this._dbContext.Reviews.Add(review);
            await this._dbContext.SaveChangesAsync();

            Assert.False(await this._ReviewsService
                .ExistsAsync(Guid.NewGuid().ToString(), review.ReceiverId));
            Assert.False(await this._ReviewsService
                .ExistsAsync(review.SenderId, Guid.NewGuid().ToString()));
            Assert.False(await this._ReviewsService
                .ExistsAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }
    }
}