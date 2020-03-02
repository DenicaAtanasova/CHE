namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using CHE.Web.InputModels.Reviews;
    using CHE.Services.Data;
    using CHE.Data.Models;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;
        private readonly UserManager<CheUser> _userManager;

        public ReviewsController(
            IReviewsService reviewsService,
            UserManager<CheUser> userManager
            )
        {
            this._reviewsService = reviewsService;
            this._userManager = userManager;
        }

        [Authorize]
        public IActionResult Send(string teacherId)
        {
            return View(new ReviewSendInputModel { ReceiverId = teacherId});
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Send(ReviewSendInputModel inputModel)
        {
            var senderId = this._userManager.GetUserId(this.User);
            var reviewSendSucceeded = await this._reviewsService
                .CreateAsync(inputModel.Comment, inputModel.Rating, senderId, inputModel.ReceiverId);
            if (!reviewSendSucceeded)
            {
                return this.BadRequest();
            }

            return this.RedirectToAction("All", "Teachers");
        }
    }
}