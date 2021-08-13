namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels.Messengers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    [Authorize]
    public class MessengersController : Controller
    {
        private readonly IMessengersService _messengersService;

        public MessengersController(
            IMessengersService messengerssService)
        {
            this._messengersService = messengerssService;
        }

        public async Task<IActionResult> Messages(
            string currentMessengerId = null,
            string receiverId = null)
        {
            var userId = this.User.GetId();

            var messenger = new MessengerPrivateUsersViewModel
            {
                Users = await this._messengersService
                .GetAllPrivateContactsByUserAsync<MessengerUserViewModel>(userId),
            };

            if (currentMessengerId != null)
            {
                messenger.CurrentMessenger = await this._messengersService
                    .GetPrivateMessengerAsync<MessengerPrivateViewModel>(userId, receiverId);
                messenger.CurrentMessenger.CurrentUser = receiverId;
            }

            return View(messenger);
        }

        public async Task<IActionResult> GetPrivate(string receiverId)
        {
            var userId = this.User.GetId();

            var messenger = await this._messengersService
                .GetPrivateMessengerAsync<MessengerPrivateViewModel>(userId, receiverId);
            messenger.CurrentUser = this.User.Identity.Name;

            return Json(messenger);
        }
    }
}