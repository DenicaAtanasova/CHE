namespace CHE.Data.Seedeing
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(CheDbContext dbContext, IServiceProvider serviceProvider);
    }
}