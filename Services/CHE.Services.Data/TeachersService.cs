namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    using CHE.Data;
    using CHE.Services.Mapping;
    using CHE.Common;
    using CHE.Data.Models;

    public class TeachersService : ITeachersService
    {
        private readonly CheDbContext _dbContext;

        public TeachersService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var teacher = await this._dbContext.Users
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return teacher;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            int startIndex = 1, 
            int endIndex = 0, 
            string schoolLevelFilter = null)
        {
            var count = endIndex == 0
                ? await this._dbContext.Users.CountAsync()
                : endIndex;

            var filteredTeachers = this.FilterCollection(schoolLevelFilter);

            var teachers = await filteredTeachers
                .Skip((startIndex - 1) * count)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

            return teachers;
        }

        public async Task<int> Count(string schoolLevelFilter)
        {
            var filteredTachers = this.FilterCollection(schoolLevelFilter);

            return await filteredTachers.CountAsync();
        }

        private IQueryable<CheUser> FilterCollection(string schoolLevelFilter = null)
        {
            var teachers = this._dbContext.Users
                .AsNoTracking()
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE);

            if (schoolLevelFilter != null)
            {
                var schoolLevel = (SchoolLevel)Enum.Parse(typeof(SchoolLevel), schoolLevelFilter);
                teachers = teachers.Where(x => x.Portfolio.SchoolLevel == schoolLevel);
            }

            return teachers;
        }
    }
}