namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.JoinRequests;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
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
                typeof(JoinRequestCreateInputModel).Assembly,
                typeof(JoinRequestDetailsViewModel).Assembly);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectJoinRequest()
        {
            var expectedRequest = new JoinRequest
            {
                CooperativeId = Guid.NewGuid().ToString(),
                SenderId = Guid.NewGuid().ToString()
            };

            this._dbContext.JoinRequests.Add(expectedRequest);
            await this._dbContext.SaveChangesAsync();

            var actualRequest = await this._joinRequestsService
                .GetByIdAsync<JoinRequestDetailsViewModel>(expectedRequest.Id);

            Assert.Equal(expectedRequest.Id, actualRequest.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithIncorrectJoinRequestId_ShouldReturnNull()
        {
            this._dbContext.JoinRequests.Add(
                new JoinRequest
                {
                    CooperativeId = Guid.NewGuid().ToString(),
                    SenderId = Guid.NewGuid().ToString()
                });
            await this._dbContext.SaveChangesAsync();

            var actualRequest = await this._joinRequestsService
                .GetByIdAsync<JoinRequestDetailsViewModel>(Guid.NewGuid().ToString());

            Assert.Null(actualRequest);
        }

        [Fact]
        public async Task GetAllByAsync_ShouldReturnCorrectCooperativesRequests()
        {
            var serchedCooperative = new Cooperative
            {
                Name = "coop"
            };

            var requestsList = new List<JoinRequest>
            {
                new JoinRequest
                {
                    Content = "JR1",
                    Cooperative = serchedCooperative,
                    Sender = new CheUser
                    {
                        UserName = "Sender"
                    }
                },
                new JoinRequest
                {
                    Content = "JR2",
                    Cooperative = serchedCooperative,
                    Sender = new CheUser
                    {
                        UserName = "Sender"
                    }
                },
                new JoinRequest
                {
                    Content = "JR3",
                    CooperativeId = Guid.NewGuid().ToString(),
                    Sender = new CheUser
                    {
                        UserName = "Sender"
                    }
                }
            };

            this._dbContext.JoinRequests.AddRange(requestsList);

            await this._dbContext.SaveChangesAsync();

            var cooperativeRequests = await this._joinRequestsService
                .GetAllAsync<JoinRequestCooperativeAllViewModel>(serchedCooperative.Id);

            var expectedRequestsList = requestsList
                .Where(x => x.CooperativeId == serchedCooperative.Id).ToList();

            Assert.Equal(expectedRequestsList.Count(), cooperativeRequests.Count());

            var index = 0;
            foreach (var request in cooperativeRequests)
            {
                Assert.Equal(expectedRequestsList[index++].Id, request.Id);
            }
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewJoinRequest()
        {
            var senderId = Guid.NewGuid().ToString();
            var content = "Content";
            var cooperativeId = Guid.NewGuid().ToString();

            var requestId = await this._joinRequestsService
                .CreateAsync(senderId, cooperativeId, content);
            var joinRequestFromDb = await this._dbContext.JoinRequests.SingleOrDefaultAsync();

            Assert.Equal(requestId, joinRequestFromDb.Id);
            Assert.Equal(content, joinRequestFromDb.Content);
            Assert.Equal(cooperativeId, joinRequestFromDb.CooperativeId);
            Assert.Equal(senderId, joinRequestFromDb.SenderId);

            var expectedCreatedOn = DateTime.UtcNow;
            Assert.Equal(expectedCreatedOn, 
                         joinRequestFromDb.CreatedOn, 
                         new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateJoinRequest()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();

            var joinRequest = new JoinRequest
            {
                Content = "Content",
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(joinRequest);
            await this._dbContext.SaveChangesAsync(); 
            this._dbContext.Entry(joinRequest).State = EntityState.Detached;

            var content = "Updated Content";

            await this._joinRequestsService.UpdateAsync(joinRequest.Id, content);

            var expectedModifiedOnDate = DateTime.UtcNow;
            var joinRequestFromDb = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync();

            Assert.Equal(content, joinRequestFromDb.Content);

            Assert.Equal(expectedModifiedOnDate, 
                         joinRequestFromDb.ModifiedOn.Value, 
                         new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteJoinRequest()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();
            var joinRequest = new JoinRequest
            {
                Content = "Content",
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(joinRequest);
            await this._dbContext.SaveChangesAsync();
            Assert.NotEmpty(this._dbContext.JoinRequests);

            this._dbContext.Entry(joinRequest).State = EntityState.Detached;

            await this._joinRequestsService.DeleteAsync(joinRequest.Id);

            Assert.Empty(this._dbContext.JoinRequests);
        }

        [Fact]
        public async Task GetPendindRequestIdAsync_WhenRequestExists_ShouldReturnPendingRequest()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();
            var request = new JoinRequest
            {
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(request);
            await this._dbContext.SaveChangesAsync();

            var pendingRequestId = await this._joinRequestsService
                .GetPendindRequestIdAsync(senderId, cooperativeId);

            Assert.Equal(request.Id, pendingRequestId);
        }

        [Fact]
        public async Task GetPendindRequestIdAsync_WhenRequestDoesNotExists_ShouldReturnNull()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();
            var request = new JoinRequest
            {
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(request);
            await this._dbContext.SaveChangesAsync();

            Assert.Null(await this._joinRequestsService
                .GetPendindRequestIdAsync(senderId, Guid.NewGuid().ToString()));
            Assert.Null(await this._joinRequestsService
                .GetPendindRequestIdAsync(Guid.NewGuid().ToString(), cooperativeId));
            Assert.Null(await this._joinRequestsService
                .GetPendindRequestIdAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }

        [Fact]
        public async Task ExistsAsync_WhenRequestExists_ShouldReturnTrue()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();
            var request = new JoinRequest
            {
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(request);
            await this._dbContext.SaveChangesAsync();

            Assert.True(await this._joinRequestsService
                .ExistsAsync(senderId, cooperativeId));
        }

        [Fact]
        public async Task ExistsAsync_WhenRequestDoesNotExists_ShouldReturnFalse()
        {
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();
            var request = new JoinRequest
            {
                CooperativeId = cooperativeId,
                SenderId = senderId
            };

            this._dbContext.JoinRequests.Add(request);
            await this._dbContext.SaveChangesAsync();

            Assert.False(await this._joinRequestsService
                .ExistsAsync(senderId, Guid.NewGuid().ToString()));
            Assert.False(await this._joinRequestsService
                .ExistsAsync(Guid.NewGuid().ToString(), cooperativeId));
            Assert.False(await this._joinRequestsService
                .ExistsAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()));
        }
    }
}