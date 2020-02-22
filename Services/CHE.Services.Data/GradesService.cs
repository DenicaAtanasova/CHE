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

        public async Task<Grade> GetByValue(string value)
        {
            var gradeFromDb = await this._dbContext.Grades
                .FirstOrDefaultAsync(x => x.Value == value);

            return gradeFromDb;
        }

        public async Task<IEnumerable<string>> GetAllAsync()
        {
            var gradesFromDb = await this._dbContext.Grades
                .Select(x => x.Value)
                .ToListAsync();

            return gradesFromDb;
        }
    }
}