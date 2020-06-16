namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

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

        public IQueryable<TEntity> GetAll<TEntity>()
        {
            var teachers = this._dbContext.Users
                .Where(x => x.RoleName == GlobalConstants.TEACHER_ROLE)
                .To<TEntity>();

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