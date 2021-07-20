namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Addresses;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<IEnumerable<string>> GetAllCitiesAsync();

        Task<IEnumerable<string>> GetAllNeighbourhoodsAsync();

        Task<string> GetAddressIdAsync(AddressInputModel address);
    }
}