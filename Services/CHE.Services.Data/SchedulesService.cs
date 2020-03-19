namespace CHE.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using CHE.Data;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    public class SchedulesService : ISchedulesService
    {
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;

        public SchedulesService(
            CheDbContext dbContext,
            IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var schedule = await this._dbContext.Schedules
                .Where(x => x.Id == id)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return schedule;
        }
    }
}