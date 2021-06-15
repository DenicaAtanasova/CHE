namespace CHE.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using CHE.Data.Models;

    using Xunit;
    using Microsoft.EntityFrameworkCore;

    public class JoinRequestsServiceTests : BaseServiceTest
    {
        private const string CONTENT = "Test content";
        private readonly string COOPERATIVE_ID = Guid.NewGuid().ToString();
        private readonly string RECEIVER_ID = Guid.NewGuid().ToString();
        private readonly string SENDER_ID = Guid.NewGuid().ToString();

        private readonly JoinRequest testRequest;

        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestsServiceTests()
            :base()
        {
            this._joinRequestsService = this.ServiceProvider.GetRequiredService<IJoinRequestsService>();

            this.testRequest = SetJoinRequest();

            this.AddTestJoinRequestAsync().GetAwaiter().GetResult();
        }

        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsyncShouldReturnCorrectJoinRequest()
        {
            var request = await this._joinRequestsService.GetByIdAsync<JoinRequest>(this.testRequest.Id);

            Assert.Equal(this.testRequest.Id, request.Id);
        }
        #endregion

        #region GetTeacherAllAsync
        #endregion

        #region GetCooperativeAllAsync
        #endregion

        #region CreateAsync
        [Fact]
        public async Task CreateAsyncShouldCreateJoinRequest()
        {
            var createSuccessful = await this._joinRequestsService
                .CreateAsync(CONTENT, COOPERATIVE_ID, RECEIVER_ID, SENDER_ID);

            Assert.True(createSuccessful);
        }

        [Fact]
        public async Task CreateAsyncShouldSetCreatedOnDateToDateTimeUtcNow()
        {
            await this._joinRequestsService.CreateAsync(CONTENT, COOPERATIVE_ID, RECEIVER_ID, SENDER_ID);

            var expectedDate = DateTime.UtcNow;
            var actualDate = await this.DbContext.JoinRequests
                .Where(x => x.CooperativeId == COOPERATIVE_ID && x.SenderId == SENDER_ID && x.ReceiverId == RECEIVER_ID)
                .Select(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            Assert.Equal(expectedDate, actualDate, new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 100));
        }
        #endregion

        #region AcceptAsync
        #endregion

        #region RejectAsync
        #endregion

        private JoinRequest SetJoinRequest()
        {
            var request = new JoinRequest
            {
                Content = CONTENT
            };

            return request;
        }

        private async Task AddTestJoinRequestAsync()
        {
            await this.DbContext.JoinRequests.AddAsync(this.testRequest);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
