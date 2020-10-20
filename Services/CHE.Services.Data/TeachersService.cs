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

        public IQueryable<TEntity> GetAll<TEntity>()
        {
            var teachers = this._dbContext.Users
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE)
                .To<TEntity>();

            return teachers;
        }
    }
}