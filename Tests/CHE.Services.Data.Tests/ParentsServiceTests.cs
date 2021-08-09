namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Tests.Mocks;
    using Microsoft.EntityFrameworkCore;

    using Moq;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class ParentsServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IParentsService _parentsService;

        private readonly CheUser User;
        private readonly Parent Sender;

        public ParentsServiceTests()
        {
            this._dbContext = DatabaseMock.Instance;

            User = new CheUser();
            Sender = new Parent
            {
                User = this.User
            };

            this._dbContext.Parents.Add(Sender);
            this._dbContext.SaveChanges();

            this._parentsService = new ParentsService(
                this._dbContext,
                JoinRequestsServiceMock.Instance,
                CooperativesServiceMock.Instance,
                ReviewsServiceMock.Instance);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewParent()
        {
            var user = new CheUser();
            this._dbContext.Users.Add(user);
            await this._dbContext.SaveChangesAsync();

            var parentId = await this._parentsService.CreateAsync(user.Id);

            var parentFromDb = await this._dbContext.Parents
                .SingleOrDefaultAsync(x => x.UserId == user.Id);
            var expectedCreatedOnDate = DateTime.UtcNow;

            Assert.Equal(parentId, parentFromDb.Id);
            Assert.Equal(expectedCreatedOnDate,
                parentFromDb.CreatedOn,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task SendRequestAsync_WithIncorrectUserId_ShouldDoNothing()
        {
            var cooperative = new Cooperative();

            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            var content = "Content";

            await this._parentsService
                .SendRequestAsync(this.User.Id, cooperative.Id, content);

            var requestFromDb = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Sender.UserId == this.User.Id &&
                                           x.CooperativeId == cooperative.Id &&
                                           x.Content == content);

            Assert.Null(requestFromDb);
        }

        [Fact]
        public async Task SendRequestAsync_WithIncorrectCooperativeId_ShouldDoNothing()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var content = "Content";
            await this._parentsService
                .SendRequestAsync(this.User.Id, cooperativeId, content);

            var requestFromDb = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Sender.UserId == User.Id &&
                                           x.CooperativeId == cooperativeId &&
                                           x.Content == content);

            Assert.Null(requestFromDb);
        }

        [Fact]
        public async Task SendRequestAsync_WithExistingJoinRequest_ShouldDoNothing()
        {
            var cooperative = new Cooperative ();
            this._dbContext.Cooperatives.Add(cooperative);

            var content = "Content";

            var joinRequest = new JoinRequest
            {
                Sender = this.Sender,
                CooperativeId = cooperative.Id,
                Content = content
            };
            this._dbContext.JoinRequests.Add(joinRequest);
            await this._dbContext.SaveChangesAsync();

            await this._parentsService.SendRequestAsync(this.User.Id, cooperative.Id, content);

            var requestFromDb = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Sender.UserId == this.User.Id &&
                                           x.CooperativeId == cooperative.Id &&
                                           x.Content == content);
        }

        [Fact]
        public async Task AcceptRequestAsync_WithNotExistingJoinRequest_ShouldDoNothing()
        {
            var cooperative = new Cooperative ();
            this._dbContext.Cooperatives.Add(cooperative);
            await this._dbContext.SaveChangesAsync();

            await this._parentsService
                .AcceptRequestAsync(this.Sender.Id, cooperative.Id, Guid.NewGuid().ToString());

            var member = await this._dbContext.ParentsCooperatives
                .SingleOrDefaultAsync(x => x.ParentId == Sender.Id && 
                                           x.CooperativeId == cooperative.Id);

            Assert.Null(member);
        }

        [Fact]
        public async Task SendReviewAsync_WithNotExistingSender_ShouldDoNothing()
        {
            var receiver = new Teacher();
            this._dbContext.Teachers.Add(receiver);
            await this._dbContext.SaveChangesAsync();

            var comment = "Comment";
            var rating = 2;

            await this._parentsService
                .SendReviewAsync(this.Sender.UserId, receiver.Id, comment, rating);

            var reviewFromDb = await this._dbContext.Reviews
                .SingleOrDefaultAsync(x => x.SenderId == Sender.Id &&
                                           x.ReceiverId == receiver.Id);

            Assert.Null(reviewFromDb);
        }

        [Fact]
        public async Task SendReviewAsync_WithExistingReview_ShouldDoNothing()
        {
            var receiver = new Teacher();
            this._dbContext.Teachers.Add(receiver);

            var comment = "Comment";
            var rating = 2;
            var review = new Review
            {
                Receiver = receiver,
                Sender = this.Sender,
                Comment = comment,
                Rating = rating
            };
            this._dbContext.Reviews.Add(review);

            await this._dbContext.SaveChangesAsync();

            await this._parentsService
                .SendReviewAsync(this.Sender.UserId, receiver.Id, comment, rating);

            var reviewFromDb = await this._dbContext.Reviews
                .SingleOrDefaultAsync(x => x.SenderId == Sender.Id &&
                                           x.ReceiverId == receiver.Id);
        }
    }
}