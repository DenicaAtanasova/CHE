namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.Profiles;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfilesService : IProfilesService
    {
        private readonly CheDbContext _dbContext;
        private readonly IImagesService _imagesService;
        private readonly IAddressesService _addressesService;

        public ProfilesService(
            CheDbContext dbContext,
            IImagesService imagesService,
            IAddressesService addressesService)
        {
            this._dbContext = dbContext;
            this._imagesService = imagesService;
            this._addressesService = addressesService;
        }

        public async Task<TEntity> GetByUserIdAsync<TEntity>(string userId) =>
            await this._dbContext.Profiles
                .AsNoTracking()
                .Where(x => x.Owner.Id == userId)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public IEnumerable<string> GetAllSchoolLevels() =>
            Enum.GetValues(typeof(SchoolLevel))
                .Cast<SchoolLevel>()
                .Where(x => x.ToString() != "Unknown")
                .Select(x => x.ToString());

        //TODO: Add tests
        public async Task<string> CreateAsync(string userId)
        {
            var portfolio = new Profile
            {
                OwnerId = userId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Profiles.Add(portfolio);
            await this._dbContext.SaveChangesAsync();

            await this._imagesService.CreateAvatarAsync(portfolio.Id);

            return portfolio.Id;
        }

        public async Task UpdateAsync(string userId, ProfileInputModel inputModel, IFormFile imageFile)
        {
            var profileToUpdate = await this._dbContext.Profiles
                .SingleOrDefaultAsync(x => x.OwnerId == userId);

            if (profileToUpdate == null)
            {
                return;
            }

            profileToUpdate.FirstName = inputModel.FirstName;
            profileToUpdate.LastName = inputModel.LastName;
            profileToUpdate.Education = inputModel.Education;
            profileToUpdate.Experience = inputModel.Experience;
            profileToUpdate.Skills = inputModel.Skills;
            profileToUpdate.Interests = inputModel.Interests;
            profileToUpdate.Skills = inputModel.Skills;
            profileToUpdate.SchoolLevel = Enum.Parse<SchoolLevel>(inputModel.SchoolLevel);
            profileToUpdate.ModifiedOn = DateTime.UtcNow;
            profileToUpdate.AddressId = await this._addressesService
                .GetAddressIdAsync(inputModel.Address);

            this._dbContext.Update(profileToUpdate);

            await this._dbContext.SaveChangesAsync();

            if (imageFile != null)
            {
                await this._imagesService.UpdateAsync(imageFile, profileToUpdate.Id);
            }
        }
    }
}