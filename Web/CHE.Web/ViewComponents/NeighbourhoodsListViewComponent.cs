namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    using System.Linq;
    using System.Threading.Tasks;

    public class NeighbourhoodsListViewComponent : ViewComponent
    {
        private readonly IAddressesService _addressesService;

        public NeighbourhoodsListViewComponent(IAddressesService addressesService)
        {
            this._addressesService = addressesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentNeighbourhood)
        {
            var neighbpurhoodList = await this._addressesService
                .GetAllNeighbourhoodsAsync();
            neighbpurhoodList = neighbpurhoodList.Where(x => x != currentNeighbourhood);

            return this.View(neighbpurhoodList);
        }
    }
}