namespace CHE.Web
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Data.Seedeing;

    using CHE.Services.Data;
    using CHE.Services.Mapping;
    using CHE.Services.Storage;

    using CHE.Web.AuthorizationPolicies;
    using CHE.Web.Cache;
    using CHE.Web.Hubs;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Microsoft.EntityFrameworkCore;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using System.Reflection;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(this.configuration);

            services.AddDbContext<CheDbContext>(options =>
                options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<CheUser, CheRole>(options =>
            {
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

            services.AddAuthorization(options =>
                options.AddPolicy("CooperativeMembersRestricted", policy =>
                    policy.Requirements.Add(new CooperativeMembersRestrictedRequirement(services)))
                );

            services.AddSignalR();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            })
                .AddNewtonsoftJson();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddRazorPages();

            services.AddMemoryCache();

            services.AddTransient<IParentsService, ParentsService>();
            services.AddTransient<ITeachersService, TeachersService>();
            services.AddTransient<ICooperativesService, CooperativesService>();
            services.AddTransient<IJoinRequestsService, JoinRequestsService>();
            services.AddTransient<ICheUsersService, CheUsersService>();
            services.AddTransient<IProfilesService, ProfilesService>();
            services.AddTransient<IReviewsService, ReviewsService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<ISchedulesService, SchedulesService>();
            services.AddTransient<IEventsService, EventsService>();
            services.AddTransient<IAddressesService, AddressesService>();
            services.AddTransient<IFileStorage>(provider => 
                new CloudStorageService(this.configuration["BlobConnectionString"]));
            services.AddTransient<IMessengersService, MessengersService>();
            services.AddTransient<IMessagesService, MessagesService>();
            services.AddTransient<IGradesService, GradesService>();
            services.AddTransient<ISchoolLevelsService, SchoolLevelsService>();
            services.AddTransient<IAddressCache, AddressCache>();
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
                dbContext.Database.Migrate();

                new CheDbContextSeeder().SeedAsync(dbContext, serviceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessengerHub>("/messenger");

                endpoints.MapControllerRoute(
                    name: "cooperative join requests",
                    pattern: "Cooperatives/JoinRequests/{cooperativeId}",
                    defaults: new { area = "Parent", controller = "JoinRequests", action = "All" });

                endpoints.MapControllerRoute(
                    name: "cooperative members",
                    pattern: "Cooperatives/Members/{id}",
                    defaults: new { area = "Parent", controller = "Cooperatives", action = "Members" });

                endpoints.MapControllerRoute(
                    name: "cooperative messenger",
                    pattern: "Cooperatives/Messenger/{cooperativeId}",
                    defaults: new { area = "Parent", controller = "Messengers", action = "Messages" });

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