namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.InputModels.Reviews;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ReviewsController : ParentController
    {
        private readonly ICheUsersService _cheUsersService;
        private readonly UserManager<CheUser> _userManager;

        public ReviewsController(
            ICheUsersService cheUsersService,
            UserManager<CheUser> userManager)
        {
            this._cheUsersService = cheUsersService;
            _userManager = userManager;
        }

        public IActionResult Send(string teacherId) =>
            this.View(new ReviewCreateInputModel { ReceiverId = teacherId });

        [HttpPost]
        public async Task<IActionResult> Send(ReviewCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                //TODO: check why this is not working
                //return this.View(inputModel.ReceiverId);
                return this.RedirectToAction(nameof(Send), new { id = inputModel.ReceiverId });
            }

            var senderId = _userManager.GetUserId(User);
            await _cheUsersService.SendReviewAsync(senderId, inputModel);

            return this.RedirectToAction("All", "Teachers", new { area = ""});
        }
    }
}