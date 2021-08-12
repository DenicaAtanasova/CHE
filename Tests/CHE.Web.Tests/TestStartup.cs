namespace CHE.Web.Tests
{
    using CHE.Services.Data;
    using CHE.Services.Storage;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
        }
    }
}