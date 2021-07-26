namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Http;

    using System;
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

        public async Task UpdateAsync(string userId, 
            string firstName, 
            string lastName,
            string education,
            string experience,
            string skills,
            string interests,
            string schoolLevel,
            string city,
            string neighbourhood,
            IFormFile imageFile)
        {
            var profileToUpdate = await this._dbContext.Profiles
                .SingleOrDefaultAsync(x => x.OwnerId == userId);

            if (profileToUpdate == null)
            {
                return;
            }

            profileToUpdate.FirstName = firstName;
            profileToUpdate.LastName = lastName;
            profileToUpdate.Education = education;
            profileToUpdate.Experience = experience;
            profileToUpdate.Skills = skills;
            profileToUpdate.Interests = interests;
            profileToUpdate.Skills = skills;
            profileToUpdate.SchoolLevel = Enum.Parse<SchoolLevel>(schoolLevel);
            profileToUpdate.AddressId = await this._addressesService
                .GetAddressIdAsync(city, neighbourhood);
            profileToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.Update(profileToUpdate);

            await this._dbContext.SaveChangesAsync();

            if (imageFile != null)
            {
                await this._imagesService.UpdateAsync(imageFile, profileToUpdate.Id);
            }
        }
    }
}