namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;

    public class CitiesListViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public CitiesListViewComponent(IAddressesService addressesService)
        {
            this._addressesService = addressesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var citiesList = await this._addressesService
                .GetAllCitiesAsync<CooperativeAllCityViewModel>();

            return this.View(citiesList);
        }
    }
}
