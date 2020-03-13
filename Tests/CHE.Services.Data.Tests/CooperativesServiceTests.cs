namespace CHE.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class CooperativesServiceTests : BaseTest
    {
        private readonly string ID = Guid.NewGuid().ToString();
        private const string NAME = "Test name";
        private const string INFO = "Test info";
        private const string GRADE = "First";

        private readonly CooperativesService _cooperativesService;

        public CooperativesServiceTests()
            : base()
        {
            this._cooperativesService = new CooperativesService(
                this.DbContext,
                this.Mapper,
                new GradesService(this.DbContext));
        }

        // CreateAsync
        [Fact]
        public async Task CreateAsyncShouldCreateCooperative()
        {
            var createSuccessful = await this._cooperativesService
                .CreateAsync(NAME, INFO, GRADE, ID);

            Assert.True(createSuccessful);
        }

        // UpdateAsync

        // DeleteAsync

        // GetByIdAsync

        // GetAllAsync

        // GetCreatorAllByUsernameAsync

        // GetJoinRequestsAsync

        // AddMemberAsync

        // RemoveMemberAsync

        // LeaveAsync
    }
}
