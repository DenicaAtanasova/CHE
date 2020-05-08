namespace CHE.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CHE.Data;
    using CHE.Services.Mapping;

    public class SchedulesService : ISchedulesService
    {
        private readonly CheDbContext _dbContext;

        public SchedulesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var schedule = await this._dbContext.Schedules
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return schedule;
        }
    }
}