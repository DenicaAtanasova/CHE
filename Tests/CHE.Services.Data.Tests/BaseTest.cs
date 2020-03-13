namespace CHE.Services.Data.Tests
{
    using System;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;

    using CHE.Data;
    using CHE.Services.Mapping;

    public class BaseTest : IDisposable
    {
        private readonly CheDbContext _dbContext;

        public BaseTest()
        {
            this._dbContext = this.GetDbContext();
        }

        public CheDbContext DbContext => this._dbContext;

        public IMapper Mapper { get; set; }

        public void Dispose()
        {
            this._dbContext.Database.EnsureDeleted();
        }

        private CheDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<CheDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new CheDbContext(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}