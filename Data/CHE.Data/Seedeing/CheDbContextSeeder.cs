namespace CHE.Data.Seedeing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class CheDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(CheDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new GradesSeeder()
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
