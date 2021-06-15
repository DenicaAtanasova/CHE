namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;

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

        public async Task<IViewComponentResult> InvokeAsync(string currentCity)
        {
            var citiesList = await this._addressesService
                .GetAllCitiesAsync();
            citiesList = citiesList.Where(x => x != currentCity);

            return this.View(citiesList);
        }
    }
}
