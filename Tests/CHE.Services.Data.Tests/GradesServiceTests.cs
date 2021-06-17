namespace CHE.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    using CHE.Data.Models;

    public class GradesServiceTests : BaseServiceTest
    {
        private const string GRADE_VALUE = "First";
        private const int GRADE_NUMVALUE = 1;

        private readonly IGradesService _gradesService;

        public GradesServiceTests()
            : base()
        {
            this._gradesService = this.ServiceProvider.GetRequiredService<IGradesService>();

            this.AddFirstAndSecondGradesAsync().GetAwaiter().GetResult();
        }

        #region GetByValueAsync
        [Fact]
        public async Task GetByValueAsyncShouldReturnCorrectGrade()
        {
            var grade = await this._gradesService.GetIdAsync(GRADE_VALUE);

            Assert.Equal(GRADE_VALUE, grade.Value);
            Assert.Equal(GRADE_NUMVALUE, grade.NumValue);
        }
        #endregion

        #region GetAllValuesAsync
        [Fact]
        public async Task GetAllValuesAsyncShouldReturnAllGrades()
        {
            var grades = await this._gradesService.GetAllValuesAsync();

            var expectedGradesCount = 2;
            var actualGradeCount = grades.Count();

            Assert.Equal(expectedGradesCount, actualGradeCount);
        }
        #endregion

        private async Task AddFirstAndSecondGradesAsync()
        {
            var grades = new List<Grade>
            {
                new Grade
                {
                    Value = "First",
                    NumValue = 1
                },
                new Grade
                {
                    Value = "Second",
                    NumValue = 2
                }
            };

            await this.DbContext.Grades.AddRangeAsync(grades);
            await this.DbContext.SaveChangesAsync();
        }
    }
}