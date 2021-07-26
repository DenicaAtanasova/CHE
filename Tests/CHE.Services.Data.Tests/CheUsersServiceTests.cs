namespace CHE.Services.Data.Tests
{
    using CHE.Data;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Xunit;

    public class CheUsersServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly ICheUsersService _cheUsersService;

        public CheUsersServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            var cooperativesService = new Mock<ICooperativesService>();
            var joinRequestsService = new Mock<IJoinRequestsService>();
            var reviewsService = new Mock<IReviewsService>();
            //this._cheUsersService = new CheUsersService(this._dbContext);
        }


        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectCooperative()
        {
        }
    }
}