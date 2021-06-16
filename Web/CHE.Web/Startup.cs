using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

using System.Reflection;

using CHE.Data;
using CHE.Data.Models;
using CHE.Data.Seedeing;
using CHE.Services.Data;
using CHE.Services.Mapping;
using CHE.Web.ViewModels;

using CHE.Web.InputModels.Cooperatives;

namespace CHE.Web
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CheDbContext>(options =>
                options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<CheUser, CheRole>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
            })
                .AddEntityFrameworkStores<CheDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddControllersWithViews()
                .AddNewtonsoftJson();

            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Application services
            services.AddTransient<ICooperativesService, CooperativesService>();
            services.AddTransient<IGradesService, GradesService>();
            services.AddTransient<IJoinRequestsService, JoinRequestsService>();
            services.AddTransient<ICheUsersService, CheUsersService>();
            services.AddTransient<IPortfoliosService, PortfoliosService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<ISchedulesService, SchedulesService>();
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IAddressesService, AddressesService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(
                typeof(ErrorViewModel).GetTypeInfo().Assembly, 
                typeof(CooperativeCreateInputModel).GetTypeInfo().Assembly);

            //Seed data
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<CheDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new CheDbContextSeeder().SeedAsync(dbContext, serviceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "parentarea",
                    pattern: "{area:exists}/{controller=home}/{action=index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=home}/{action=index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}