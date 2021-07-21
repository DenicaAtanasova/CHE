namespace CHE.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<IEnumerable<string>> GetAllCitiesAsync();

        Task<IEnumerable<string>> GetAllNeighbourhoodsAsync();

        Task<string> GetAddressIdAsync(string city, string neighbourhood);
    }
}