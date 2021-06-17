namespace CHE.Services.Data
{
    using CHE.Data;

    using Microsoft.EntityFrameworkCore;
    
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GradesService : IGradesService
    {
        private readonly CheDbContext _dbContext;

        public GradesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<string> GetGardeIdAsync(string value)
            => await this._dbContext.Grades
                .AsNoTracking()
                .Where(x => x.Value == value)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<string>> GetAllValuesAsync(string currentGrade = null)
            => await this._dbContext.Grades
                .AsNoTracking()    
                .Where(x => x.Value != currentGrade)
                .OrderBy(x => x.NumValue)
                .Select(x => x.Value)
                .ToListAsync();
    }
}