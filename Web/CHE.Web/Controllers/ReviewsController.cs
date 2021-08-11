namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels.Reviews;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    using static WebConstants;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this._reviewsService = reviewsService;
        }

        [HttpGet("Teachers/[Controller]/{id}")]
        public async Task<IActionResult> All(string id)
        {
            ViewData["id"] = id;
            this.ViewData["layout"] = TeacherLayout;

            return View(new ReviewAllListViewModel
            {
                Reviews = await this._reviewsService
                .GetAllByReceiverAsync<ReviewAllViewModel>(id)
            });
        }

        [Authorize]
        [HttpGet("Identity/Account/Manage/[Controller]")]
        public async Task<IActionResult> MyAll()
        {
            var id = this.User.GetId();

            ViewData["id"] = id;
            this.ViewData["layout"] = AccountLayout;

            return View("All", new ReviewAllListViewModel
            {
                Reviews = await this._reviewsService
                .GetAllByReceiverAsync<ReviewAllViewModel>(id)
            });
        }
    }
}