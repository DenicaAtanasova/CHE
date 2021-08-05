namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Messengers;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class MessengersController : ParentController
    {
        private readonly IMessengersService _messengersService;

        public MessengersController(IMessengersService messengersService)
        {
            this._messengersService = messengersService;
        }

        public async Task<IActionResult> Messages(string cooperativeId)
        {
            var messenger = await this._messengersService
                .GetCooperativeMessengerWithUsersAsync<MessengerCooperativeUsersViewModel>(cooperativeId);

            this.ViewData["id"] = cooperativeId;

            return View(messenger);
        }

        public async Task<IActionResult> Cooperative(string cooperativeId)
        {
            var messenger = await this._messengersService
                .GetCooperativeMessengerWithMessagesAsync<MessengerCooperativeViewModel>(cooperativeId);
            messenger.CurrentUser = this.User.Identity.Name;

            return Json(messenger);
        }
    }
}