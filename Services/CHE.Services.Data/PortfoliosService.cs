namespace CHE.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;

    public class PortfoliosService : IPortfoliosService
    {
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;

        public PortfoliosService(
            CheDbContext dbContext,
            IMapper mapper)
            {
                this._dbContext = dbContext;
                this._mapper = mapper;
            }

        public async Task<TEntity> GetByUserIdAsync<TEntity>(string userId)
        {
            var portfolio = await this._dbContext.Portfolios
                .Where(x => x.Owner.Id == userId)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return portfolio;
        }
    }
}