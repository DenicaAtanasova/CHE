namespace CHE.Web.Areas.Parent.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;

    using CHE.Web.InputModels.Reviews;
    using CHE.Services.Data;
    using CHE.Data.Models;

    public class ReviewsController : ParentController
    {
        private readonly IReviewsService _reviewsService;
        private readonly UserManager<CheUser> _userManager;

        public ReviewsController(
            IReviewsService reviewsService,
            UserManager<CheUser> userManager
            )
        {
            _reviewsService = reviewsService;
            _userManager = userManager;
        }

        public IActionResult Send(string teacherId)
        {
            return View(new ReviewSendInputModel { ReceiverId = teacherId });
        }

        [HttpPost]
        public async Task<IActionResult> Send(ReviewSendInputModel inputModel)
        {

            var senderId = _userManager.GetUserId(User);
            var reviewSendSucceeded = await _reviewsService
                .CreateAsync(inputModel.Comment, inputModel.Rating, senderId, inputModel.ReceiverId);
            if (!reviewSendSucceeded)
            {
                return BadRequest();
            }

            return RedirectToAction("All", "Teachers", new { area = ""});
        }
    }
}