namespace CHE.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;
    using System;

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
            var portfolioToUpdate = await this._dbContext.Portfolios
                .SingleOrDefaultAsync(x => x.OwnerId == teacherId);

            portfolioToUpdate.ModifiedOn = DateTime.UtcNow;
            portfolioToUpdate.FirstName = updatedPortfolio.FirstName;
            portfolioToUpdate.LastName = updatedPortfolio.LastName;
            portfolioToUpdate.Education = updatedPortfolio.Education;
            portfolioToUpdate.Skills = updatedPortfolio.Skills;
            portfolioToUpdate.Experience = updatedPortfolio.Experience;
            portfolioToUpdate.Interests = updatedPortfolio.Interests;

            this._dbContext.Update(portfolioToUpdate);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }
    }
}