namespace CHE.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;

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

        public async Task<bool> UpdateAsync<TEntity>(string teacherId, TEntity portfolio)
        {
            var updatedPortfolio = this._mapper.Map<TEntity, Portfolio>(portfolio);
            var portfolioFromDb = await this._dbContext.Portfolios
                .SingleOrDefaultAsync(x => x.OwnerId == teacherId);

            this._dbContext.Entry(portfolioFromDb).State = EntityState.Detached;

            updatedPortfolio.Id = portfolioFromDb.Id;
            updatedPortfolio.CreatedOn = portfolioFromDb.CreatedOn;
            updatedPortfolio.OwnerId = portfolioFromDb.OwnerId;
            updatedPortfolio.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Update(updatedPortfolio);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }
    }
}