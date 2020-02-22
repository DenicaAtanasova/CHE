namespace CHE.Services.Data
{
    using CHE.Data.Models;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<Address> CreateAsync(string city, string neighbourhood, string street = null);

        Task DeleteAsync(string id);
    }
}