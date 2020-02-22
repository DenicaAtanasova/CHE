namespace CHE.Services.Data
{
    using CHE.Data;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using CHE.Data.Models;

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

        public Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            throw new System.NotImplementedException();
        }
    }
}