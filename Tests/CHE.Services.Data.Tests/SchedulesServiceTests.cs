namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.ViewModels.Schedules;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class SchedulesServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly ISchedulesService _schedulesService;

        public SchedulesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            this._schedulesService = new SchedulesService(this._dbContext);

            AutoMapperConfig.RegisterMappings(typeof(ScheduleViewModel).Assembly);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectJoinRequest()
        {
            var expectedSchedule = new Schedule
            { 
                CooperativeId = Guid.NewGuid().ToString()
            };

            this._dbContext.Schedules.Add(expectedSchedule);
            await this._dbContext.SaveChangesAsync();

            var actualSchedule = await this._schedulesService
                .GetByIdAsync<ScheduleViewModel>(expectedSchedule.Id);

            Assert.Equal(expectedSchedule.Id, actualSchedule.Id);
            Assert.Equal(expectedSchedule.CooperativeId, actualSchedule.CooperativeId);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNullWithIncorrectId()
        {
            var expectedSchedule = new Schedule
            {
                CooperativeId = Guid.NewGuid().ToString()
            };

            this._dbContext.Schedules.Add(expectedSchedule);
            await this._dbContext.SaveChangesAsync();

            var actualRequest = await this._schedulesService
                .GetByIdAsync<ScheduleViewModel>(Guid.NewGuid().ToString());

            Assert.Null(actualRequest);
        }

        [Fact]
        public async Task GetIdByCooperative_ShouldWorkCorrectly()
        {
            var CooperativeId = Guid.NewGuid().ToString();
            var schedule = new Schedule
            {
                CooperativeId = CooperativeId,
                TeacherId = Guid.NewGuid().ToString()
            };

            this._dbContext.Schedules.Add(schedule);
            await this._dbContext.SaveChangesAsync();

            var scheduleId = await this._schedulesService.GetIdByCooperativeAsync(CooperativeId);

            Assert.Equal(schedule.Id, scheduleId);
        }
    }
}
