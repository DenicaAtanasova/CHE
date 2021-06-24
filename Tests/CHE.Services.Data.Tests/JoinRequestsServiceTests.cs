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
        public async Task CreateAsync_ShouldWorkCorrectly()
        {
            var content = "Content";
            var cooperativeId = Guid.NewGuid().ToString();
            var senderId = Guid.NewGuid().ToString();

            var requestId = await this._joinRequestsService.CreateAsync(content, cooperativeId, senderId);
            var joinRequestFromDb = await this._dbContext.JoinRequests.SingleOrDefaultAsync();

            Assert.Equal(joinRequestFromDb.Id, requestId);
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

            Assert.Equal(expectedDate, actualDate, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }

        [Fact]
        public async Task GetAllByTeacherAsync_ShouldWorkCorrectly()
        {
            var serchedTeacher = new CheUser
            {
                UserName = "Maria"
            };

            var requestsList = new List<JoinRequest>
            {
                new JoinRequest
                {
                    Content = "JR1",
                    Receiver = serchedTeacher,
                    Sender = new CheUser
                    {
                        UserName = "Sender"
                    }
                },
                new JoinRequest
                {
                    Content = "JR2",
                    Receiver = serchedTeacher,
                    Sender = new CheUser
                    {
                        UserName = "Sender"
                    }
                },
                new JoinRequest
                {
                    Content = "JR3",
                    ReceiverId = Guid.NewGuid().ToString(),
                    Sender = new CheUser
                    {
                        UserName = "Sender"
                    }
                }
            };

            this._dbContext.JoinRequests.AddRange(requestsList);

            await this._dbContext.SaveChangesAsync();

            var teachersRequests = await this._joinRequestsService
                .GetAllByTeacherAsync<JoinRequestAllViewModel>(serchedTeacher.Id);
            var expectedrequestsList = requestsList.Where(x => x.ReceiverId == serchedTeacher.Id).ToList();

            Assert.Equal(expectedrequestsList.Count(), teachersRequests.Count());

            var index = 0;
            foreach (var request in teachersRequests)
            {
                Assert.Equal(requestsList[index++].Id, request.Id);
            }
        }

        [Fact]
        public async Task GetAllByCooperativeAsync_ShouldWorkCorrectly()
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
                .GetAllByCooperativeAsync<JoinRequestAllViewModel>(serchedCooperative.Id);
            var expectedRequestsList = requestsList.Where(x => x.CooperativeId == serchedCooperative.Id).ToList();

            Assert.Equal(expectedRequestsList.Count(), cooperativeRequests.Count());

            var index = 0;
            foreach (var request in cooperativeRequests)
            {
                Assert.Equal(requestsList[index++].Id, request.Id);
            }
        }
    }
}