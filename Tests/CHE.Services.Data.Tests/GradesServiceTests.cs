namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class GradesServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IGradesService _gradesService;

        public GradesServiceTests()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._dbContext = new CheDbContext(options);

            this._gradesService = new GradesService(this._dbContext);
        }
        [Theory]
        [InlineData("First")]
        [InlineData("Second")]
        public async Task GetGardeIdAsync_ShouldWorkCorrectly(string gradeValue)
        {
            var gradesList = new string[]
            {
                "First",
                "Second",
                "Third",
                "Forth"
            };

            var grades = new List<Grade>();
            for (int i = 1; i <= gradesList.Length; i++)
            {
                grades.Add(
                    new Grade
                    {
                        NumValue = i,
                        Value = gradesList[i - 1]
                    });
            }

            this._dbContext.Grades.AddRange(grades);
            await this._dbContext.SaveChangesAsync();

            var gradeFromDb = await this._dbContext.Grades
                .SingleOrDefaultAsync(x => x.Value == gradeValue);
            var gradeId = await this._gradesService.GetGardeIdAsync(gradeValue);

            Assert.Equal(gradeFromDb.Id, gradeId);
        }

        [Fact]
        public async Task GetAllValuesAsync_ShouldWorkCorrectlyWithCurrentGradeNull()
        {
            var gradesList = new string[]
            {
                "First",
                "Second",
                "Third",
                "Forth"
            };

            var grades = new List<Grade>();
            for (int i = 1; i <= gradesList.Length; i++)
            {
                grades.Add(
                    new Grade
                    {
                        NumValue = i,
                        Value = gradesList[i - 1]
                    });
            }

            this._dbContext.Grades.AddRange(grades);
            await this._dbContext.SaveChangesAsync();

            var gradesValues = await this._gradesService.GetAllValuesAsync();

            Assert.Equal(gradesList, gradesValues);
        }

        [Theory]
        [InlineData("First")]
        [InlineData("Second")]
        public async Task GetAllValuesAsync_ShouldWorkCorrectlyWithCurrentGradeNotNull(string currentGrade)
        {
            var gradesList = new string[]
            {
                "First",
                "Second",
                "Third",
                "Forth"
            };

            var grades = new List<Grade>();
            for (int i = 1; i <= gradesList.Length; i++)
            {
                grades.Add(
                    new Grade
                    {
                        NumValue = i,
                        Value = gradesList[i - 1]
                    });
            }

            this._dbContext.Grades.AddRange(grades);
            await this._dbContext.SaveChangesAsync();

            var expectedGradesValues = gradesList
                .Where(x => x != currentGrade);

            var actualGradesValues = await this._gradesService
                .GetAllValuesAsync(currentGrade);

            Assert.Equal(expectedGradesValues, actualGradesValues);
        }
    }
}