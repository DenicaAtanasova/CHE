namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
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

        public async Task<IActionResult> Details(string cooperativeId)
        {
            var messenger = await this._messengersService
                .GetCooperativeMessengerAsync<MessengerCooperativeUsersViewModel>(cooperativeId);

            this.ViewData["id"] = cooperativeId;

            return View(messenger);
        }

        public async Task<IActionResult> GetCooperative(string cooperativeId)
        {
            var messenger = await this._messengersService
                .GetCooperativeMessengerAsync<MessengerCooperativeViewModel>(cooperativeId);
            messenger.CurrentUser = this.User.Identity.Name;

            return Json(messenger);
        }

        public async Task<IActionResult> SendToMessenger(string receiverId)
        {
            var userId = this.User.GetId();

            var messengerId = await this._messengersService
                .GetPrivateMessengerIdAsync(userId, receiverId);

            return this.RedirectToAction(
                "Details", 
                "Messengers", 
                new { area = "", currentMessengerId = messengerId, receiverId = receiverId });
        }
    }
}