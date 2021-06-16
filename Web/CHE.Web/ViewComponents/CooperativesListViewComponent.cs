namespace CHE.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.JoinRequests;

    public class CooperativesListViewComponent : ViewComponent
    {
        private readonly ICooperativesService _cooperativesService;

        public CooperativesListViewComponent(ICooperativesService cooperativesService)
        {
            this._cooperativesService = cooperativesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string creatorUsername)
        {
            var cooperativesList = await this._cooperativesService
                .GetAllByCreatorAsync<JoinRequestCooperativeSendViewModel>(creatorUsername);

            return this.View(cooperativesList);
        }
    }
}
