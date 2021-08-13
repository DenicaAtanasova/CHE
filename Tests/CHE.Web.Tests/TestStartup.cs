namespace CHE.Web.Tests
{
    using CHE.Services.Data;
    using CHE.Services.Storage;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MyTested.AspNetCore.Mvc;
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public void ConfigureTestServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.ReplaceTransient<IReviewsService>(_ =>MockProvider.ReviewsService());
            services.ReplaceTransient<IParentsService>(_ => MockProvider.ParentsService());
            services.ReplaceTransient<ITeachersService>(_ => MockProvider.TeachersService());
            services.ReplaceTransient<IFileStorage>(_ => MockProvider.CloudStorageService());
            services.ReplaceTransient<IMessengersService>(_ => MockProvider.MessengersService());
            services.ReplaceTransient<IJoinRequestsService>(_ => MockProvider.JoinRequestsService());
            services.ReplaceTransient<ICooperativesService>(_ => MockProvider.CooperativesService());
            services.ReplaceTransient<ISchedulesService>(_ => MockProvider.SchedulesService());
        }
    }
}