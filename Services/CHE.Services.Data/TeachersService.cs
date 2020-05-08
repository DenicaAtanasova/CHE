namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    using CHE.Data;
    using CHE.Services.Mapping;
    using CHE.Common;

    public class TeachersService : ITeachersService
    {
        private readonly CheDbContext _dbContext;

        public TeachersService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            var teachers = await this._dbContext.Users
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE)
                .To<TEntity>()
                .ToArrayAsync();

            return teachers;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var teacher = await this._dbContext.Users
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return teacher;
        }
    }
}