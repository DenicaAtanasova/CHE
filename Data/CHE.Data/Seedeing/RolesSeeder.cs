namespace CHE.Data.Seedeing
{
    using Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class RolesSeeder : ISeeder
    {
        //TODO: Check if role names have to be global constants
        private const string TEACHER_ROLE = "Teacher";
        private const string PARENT_ROLE = "Parent";

        public async Task SeedAsync(CheDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<CheRole>>();

            await SeedRoleAsync(roleManager, TEACHER_ROLE);
            await SeedRoleAsync(roleManager, PARENT_ROLE);
        }

        private static async Task SeedRoleAsync(RoleManager<CheRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new CheRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(err => err.Description)));
                }
            }
        }
    }
}