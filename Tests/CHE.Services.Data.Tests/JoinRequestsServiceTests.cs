namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.JoinRequests;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class JoinRequestsServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IJoinRequestsService _joinRequestsService;


        public JoinRequestsServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            this._joinRequestsService = new JoinRequestsService(this._dbContext);

            AutoMapperConfig.RegisterMappings(
                typeof(JoinRequestInputModel).Assembly,
                typeof(JoinRequestDetailsViewModel).Assembly);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectJoinRequest()
        {
            var expectedRequest = new JoinRequest
            {
                CooperativeId = Guid.NewGuid().ToString(),
                ReceiverId = Guid.NewGuid().ToString()
            };

            this._dbContext.JoinRequests.Add(expectedRequest);
            await this._dbContext.SaveChangesAsync();

            var actualRequest = await this._joinRequestsService
                .GetByIdAsync<JoinRequestDetailsViewModel>(expectedRequest.Id);

            Assert.Equal(expectedRequest.Id, actualRequest.Id);
            Assert.Equal(expectedRequest.CooperativeId, actualRequest.CooperativeId);
            Assert.Equal(expectedRequest.ReceiverId, actualRequest.ReceiverId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullWithIncorrectId()
        {
            this._dbContext.JoinRequests.Add(
                new JoinRequest
                {
                    CooperativeId = Guid.NewGuid().ToString(),
                    ReceiverId = Guid.NewGuid().ToString()
                });
            await this._dbContext.SaveChangesAsync();

            var actualRequest = await this._joinRequestsService
                .GetByIdAsync<JoinRequestDetailsViewModel>(Guid.NewGuid().ToString());

            Assert.Null(actualRequest);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectlyWithoutExistingJoinRequest()
        {
            var content = "Content";
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();

            var requestId = await this._joinRequestsService.CreateAsync(content, cooperativeId, senderId);
            var joinRequestFromDb = await this._dbContext.JoinRequests.SingleOrDefaultAsync();

            Assert.Equal(joinRequestFromDb.Id, requestId);
        }

        [Fact]
        public async Task CreateAsync_ShouldWorkCorrectlyWithExistingJoinRequest()
        {

            var content = "Content";
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();

            var testJoinRequest = new JoinRequest
            {
                Content = content,
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(testJoinRequest);
            await this._dbContext.SaveChangesAsync();

            var requestId = await this._joinRequestsService.CreateAsync(content, cooperativeId, senderId);

            Assert.Equal(testJoinRequest.Id, requestId);
        }

        [Fact]
        public async Task CreateAsync_ShouldSetCreatedOnDateToDateTimeUtcNow()
        {
            var content = "Content";
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();

            await this._joinRequestsService.CreateAsync(content, cooperativeId, senderId);

            var expectedDate = DateTime.UtcNow;
            var actualDate = await this._dbContext.JoinRequests
                .Where(x => x.CooperativeId == cooperativeId && x.SenderId == senderId)
                .Select(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            Assert.Equal(expectedDate, actualDate, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 500));
        }
    }
}