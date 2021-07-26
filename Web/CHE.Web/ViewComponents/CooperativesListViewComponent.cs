namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using CHE.Services.Data.Models;
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
                .GetAllByUserAsync<JoinRequestCooperativeSendViewModel>(userId, CooperativeUser.Admin);

            return this.View(cooperativesList);
        }
    }
}