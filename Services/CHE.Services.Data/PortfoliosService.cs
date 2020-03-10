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
    using Microsoft.AspNetCore.Http;

    public class PortfoliosService : IPortfoliosService
    {
        private const string DEFAULT_IMAGE_CAPTION = "Teacher_Avatar.png";
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IImagesService _imagesService;

        public PortfoliosService(
            CheDbContext dbContext,
            IMapper mapper,
            IImagesService imagesService)
            {
                this._dbContext = dbContext;
                this._mapper = mapper;
            this._imagesService = imagesService;
        }

        public async Task<TEntity> GetByUserIdAsync<TEntity>(string userId)
        {
            var portfolio = await this._dbContext.Portfolios
                .Where(x => x.Owner.Id == userId)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return portfolio;
        }

        public async Task<bool> UpdateAsync<TEntity>(string teacherId, TEntity portfolio, IFormFile imageFile)
        {
            var updatedPortfolio = this._mapper.Map<TEntity, Portfolio>(portfolio);
            var portfolioFromDb = await this._dbContext.Portfolios
                .SingleOrDefaultAsync(x => x.OwnerId == teacherId);

            this._dbContext.Entry(portfolioFromDb).State = EntityState.Detached;

            if (imageFile != null && imageFile.FileName != DEFAULT_IMAGE_CAPTION)
            {
                //TODO: check result
                await this._imagesService.Update(imageFile, portfolioFromDb.Id);
            }
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