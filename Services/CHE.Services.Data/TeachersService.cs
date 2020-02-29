namespace CHE.Services.Data
{
    using Microsoft.EntityFrameworkCore;

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;

    public class TeachersService : ITeachersService
    {
        private const string TEACHER_ROLE = "Teacher";

        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;

        public TeachersService(
            CheDbContext dbContext,
            IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            var teachers = await this._dbContext.Users
                .Where(x => x.RoleName == TEACHER_ROLE)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .ToArrayAsync();

            return teachers;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var teacher = await this._dbContext.Users
                .Where(x => x.Id == id)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return teacher;
        }
    }
}