namespace CHE.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;

    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class PortfoliosService : IPortfoliosService
    {
        private readonly CheDbContext _dbContext;
        private readonly IImagesService _imagesService;

        public PortfoliosService(
            CheDbContext dbContext,
            IImagesService imagesService)
        {
            this._dbContext = dbContext;
            this._imagesService = imagesService;
        }

        public async Task<TEntity> GetByUserIdAsync<TEntity>(string userId)
        {
            var portfolio = await this._dbContext.Portfolios
                .Where(x => x.Owner.Id == userId)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return portfolio;
        }

        public async Task<bool> UpdateAsync<TEntity>(string teacherId, TEntity portfolio, IFormFile imageFile)
        {
            var updatedPortfolio = portfolio.Map<TEntity, Portfolio>();
            var portfolioFromDb = await this._dbContext.Portfolios
                .SingleOrDefaultAsync(x => x.OwnerId == teacherId);

            this._dbContext.Entry(portfolioFromDb).State = EntityState.Detached;

            if (imageFile != null)
            {
                //TODO: check result
                await this._imagesService.UpdateAsync(imageFile, portfolioFromDb.Id);
            }
            updatedPortfolio.Id = portfolioFromDb.Id;
            updatedPortfolio.CreatedOn = portfolioFromDb.CreatedOn;
            updatedPortfolio.OwnerId = portfolioFromDb.OwnerId;
            updatedPortfolio.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Update(updatedPortfolio);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public IEnumerable<string> GetAllSchoolLevels(string currentSchoolLevel)
        {
            var schoolLevelList = Enum.GetValues(typeof(SchoolLevel))
                .Cast<SchoolLevel>()
                .Where(x => x.ToString() != "Unknown")
                .Select(x => x.ToString());

            if (!string.IsNullOrEmpty(currentSchoolLevel))
            {
                schoolLevelList = schoolLevelList
                    .Where(x => x.ToString() == currentSchoolLevel);
            }

            return schoolLevelList;
        }
    }
}