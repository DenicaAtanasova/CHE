namespace CHE.Services.Data
{
    using CHE.Web.InputModels.Cooperatives;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAddressesService
    {
        Task<IEnumerable<string>> GetAllCitiesAsync();

        Task<IEnumerable<string>> GetAllNeighbourhoodsAsync();

        Task<string> GetAddressIdAsync(CooperativeAddressInputModel address);
    }
}