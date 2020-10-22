namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Cooperatives;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class NeighbourhoodsListViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public NeighbourhoodsListViewComponent(IAddressesService addressesService)
        {
            this._addressesService = addressesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var neighbpurhoodList = await this._addressesService
                .GetAllNeighbourhoodsAsync<CooperativeAllNeighbourhoodViewModel>();

            return this.View(neighbpurhoodList);
        }
    }
}
