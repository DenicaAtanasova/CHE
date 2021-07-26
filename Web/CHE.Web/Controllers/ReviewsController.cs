namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels.Reviews;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    [Authorize]
    public class ReviewsController : Controller
    {
        private const string ACCOUNT_LAYOUT = "/Areas/Identity/Pages/Account/Manage/_Layout.cshtml";
        private const string TEACHER_LAYOUT = "/Views/Shared/_LayoutTeacher.cshtml";

        private readonly IReviewsService _reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this._reviewsService = reviewsService;
        }

        public async Task<IActionResult> All(string id)
        {
            if (id == null)
            {
                id = this.User.GetId();
                this.ViewData["layout"] = ACCOUNT_LAYOUT;
            }
            else
            {
                this.ViewData["layout"] = TEACHER_LAYOUT;
            }

            ViewData["id"] = id;

            return View(new ReviewAllListViewModel
            {
                Reviews = await this._reviewsService.GetAllByReceiverAsync<ReviewAllViewModel>(id)
            });
        }
    }
}