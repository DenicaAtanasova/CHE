namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using System.Linq;

    public class GradesService : IGradesService
    {
        private readonly CheDbContext _dbContext;

        public GradesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Grade> GetByValueAsync(string value)
        {
            var gradeFromDb = await this._dbContext.Grades
                .SingleOrDefaultAsync(x => x.Value == value);

            return gradeFromDb;
        }

        public async Task<IEnumerable<string>> GetAllValuesAsync(string currentGrade = null)
        {
            var gradesFromDb = await this._dbContext.Grades
                .Where(x => x.Value != currentGrade)
                .OrderBy(x => x.NumValue)
                .Select(x => x.Value)
                .ToArrayAsync();

            return gradesFromDb;
        }
    }
}