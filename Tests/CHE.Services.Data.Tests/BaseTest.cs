namespace CHE.Services.Data.Tests
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using AutoMapper;

    using CHE.Services.Mapping;
    using CHE.Data;

    public class BaseTest : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;

        public BaseTest()
        {
            this._serviceProvider = this.GetServiceProvider();

            this._dbContext = this._serviceProvider.GetRequiredService<CheDbContext>();
            this._mapper = this._serviceProvider.GetRequiredService<IMapper>();
        }

        public CheDbContext DbContext => this._dbContext;

        public IMapper Mapper => this._mapper;

        public void Dispose()
        {
            this._dbContext.Database.EnsureDeleted();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            // dbContext
            services.AddDbContext<CheDbContext>(
                opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            // autoMapper
            services.AddAutoMapper(typeof(CooperativeProfile));

            return services;
        }

        private ServiceProvider GetServiceProvider()
        {
            var services = this.SetServices();

            return services.BuildServiceProvider();
        }
    }
}