namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeachersService : ITeachersService
    {
        private readonly CheDbContext _dbContext;
        private readonly IProfilesService _profilesService;

        public TeachersService(CheDbContext dbContext, 
            IProfilesService profilesService)
        {
            this._dbContext = dbContext;
            this._profilesService = profilesService;
        }

        public async Task<string> CreateAsync(string userId)
        {
            var teacher = new Teacher
            {
                UserId = userId,
                CreatedOn = DateTime.UtcNow,
                Schedule = new Schedule
                {
                    CreatedOn = DateTime.UtcNow
                }
            };

            this._dbContext.Teachers.Add(teacher);
            await this._dbContext.SaveChangesAsync();

            await this._profilesService.CreateAsync(teacher.Id);

            return teacher.Id;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id) =>
            await this._dbContext.Teachers
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1,
            int endIndex = 0,
            string schoolLevel = null,
            string city = null,
            string neighbourhood = null)
        {
            var filteredTeachers = this.GetFilteredCollection(schoolLevel, city, neighbourhood);

            var count = endIndex == 0
                ? await filteredTeachers.CountAsync()
                : endIndex;

            return await filteredTeachers
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();
        }

        public async Task<int> CountAsync(
            string schoolLevel= null,
            string city = null,
            string neighbourhood = null) =>
                await this.GetFilteredCollection(schoolLevel, city, neighbourhood)
                    .CountAsync();

        private IQueryable<Teacher> GetFilteredCollection(
            string schoolLevelFilter = null,
            string cityFilter = null,
            string neighbourhoodFilter = null)
        {
            var users = this._dbContext.Teachers.AsNoTracking();

            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                users = users.Where(x => x.Profile.SchoolLevel == schoolLevel);
            }

            if (cityFilter != null)
            {
                users = users.Where(x => x.Profile.Address.City == cityFilter);
            }

            if (neighbourhoodFilter != null)
            {
                users = users.Where(x => x.Profile.Address.Neighbourhood == neighbourhoodFilter);
            }

            return users;
        }
    }
}