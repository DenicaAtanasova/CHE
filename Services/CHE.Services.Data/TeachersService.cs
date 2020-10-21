namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;

    using CHE.Data;
    using CHE.Services.Mapping;
    using CHE.Common;

    //TODO: Rename to UserService
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

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(int startIndex = 1, int endIndex = 0)
        {
            var count = endIndex == 0
                ? await this._dbContext.Users.CountAsync()
                : endIndex;

            var teachers = await this._dbContext.Users
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE)
                .Skip(startIndex - 1)
                .Take(count)
                .To<TEntity>()
                .ToListAsync();

            return teachers;
        }

        public async Task<int> Count() =>
            await this._dbContext.Users
            .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE)
            .CountAsync();
    }
}