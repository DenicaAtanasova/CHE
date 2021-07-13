namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.JoinRequests;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class CooperativesListViewComponent : ViewComponent
    {
        private readonly ICooperativesService _cooperativesService;

        public CooperativesListViewComponent(ICooperativesService cooperativesService)
        {
            this._cooperativesService = cooperativesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var cooperativesList = await this._cooperativesService
                .GetAllByAdminAsync<JoinRequestCooperativeSendViewModel>(userId);

            return this.View(cooperativesList);
        }
    }
}