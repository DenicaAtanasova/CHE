namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels.Messengers;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class MessengersController : Controller
    {
        private readonly IMessengersService _messengersService;

        public MessengersController(IMessengersService messengerssService)
        {
            this._messengersService = messengerssService;
        }

        public async Task<IActionResult> Messages()
        {
            var userId = this.User.GetId();
            var messengers = await this._messengersService
                .GetAllPrivateMessengersByUserAsync<MessengerUserViewModel>(userId);

            return View(messengers);
        }

        public async Task<IActionResult> Private(string receiverId)
        {
            var userId = this.User.GetId();

            var messenger = await this._messengersService
                .GetPrivateMessengerWithMessagesAsync<MessengerPrivateViewModel>(userId, receiverId);
            messenger.CurrentUser = this.User.Identity.Name;

            return Json(messenger);
        }
    }
}
