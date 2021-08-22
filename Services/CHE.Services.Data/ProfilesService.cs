namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.IO;
    using CHE.Data.Models.Enums;

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
                .Where(x => x.Owner.UserId == userId)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<string> CreateAsync(string userId)
        {
            var profile = new Profile
            {
                OwnerId = userId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.Profiles.Add(profile);
            await this._dbContext.SaveChangesAsync();

            await this._imagesService.CreateAvatarAsync(profile.Id);

            return profile.Id;
        }

        public async Task UpdateAsync(
            string id, 
            string firstName, 
            string lastName,
            string education,
            string experience,
            string skills,
            string interests,
            string schoolLevel,
            string city,
            string neighbourhood,
            Stream imageFile)
        {
            var profileToUpdate = await this._dbContext.Profiles
                .SingleOrDefaultAsync(x => x.Id == id);

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
            profileToUpdate.ModifiedOn = DateTime.UtcNow;

            if (city != null)
            {
                profileToUpdate.AddressId = await this._addressesService
                    .GetAddressIdAsync(city, neighbourhood);
            }

            this._dbContext.Update(profileToUpdate);

            await this._dbContext.SaveChangesAsync();

            if (imageFile != null)
            {
                await this._imagesService.UpdateAsync(imageFile, profileToUpdate.Id);
            }
        }
    }
}