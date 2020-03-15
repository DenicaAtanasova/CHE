namespace CHE.Services.Data.Tests
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using AutoMapper;

    using CHE.Services.Mapping;
    using CHE.Data;

    public class BaseServiceTest : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly CheDbContext _dbContext;

        public BaseServiceTest()
        {
            this._serviceProvider = SetServices().BuildServiceProvider();

            this._dbContext = this._serviceProvider.GetRequiredService<CheDbContext>();
        }

        public CheDbContext DbContext => this._dbContext;

        public IServiceProvider ServiceProvider => this._serviceProvider;

        public void Dispose()
        {
            this._dbContext.Database.EnsureDeleted();
            this.SetServices();
        }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            // dbContext
            services.AddDbContext<CheDbContext>(
                opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            // autoMapper
            services.AddAutoMapper(typeof(CooperativeProfile));

            // Application services
            services.AddTransient<ICooperativesService, CooperativesService>();
            services.AddTransient<IGradesService, GradesService>();
            services.AddTransient<IJoinRequestsService, JoinRequestsService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<IPortfoliosService, PortfoliosService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IImagesService, ImagesService>();

            return services;
        }
    }
}