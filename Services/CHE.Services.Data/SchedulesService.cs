namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System.Linq;
    using System.Threading.Tasks;

    public class SchedulesService : ISchedulesService
    {
        private readonly CheDbContext _dbContext;

        public SchedulesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
            => await this._dbContext.Schedules
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();
    }
}