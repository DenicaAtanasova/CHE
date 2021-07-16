namespace CHE.Web.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Reviews;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ReviewsController : Controller
    {
        private const string ACCOUNT_LAYOUT = "/Areas/Identity/Pages/Account/Manage/_Layout.cshtml";
        private const string TEACHER_LAYOUT = "/Views/Shared/_LayoutTeacher.cshtml";

        private readonly UserManager<CheUser> _userManager;
        private readonly IReviewsService _reviewsService;

        public ReviewsController(
            UserManager<CheUser> userManager,
            IReviewsService reviewsService)
        {
            this._userManager = userManager;
            this._reviewsService = reviewsService;
        } 

        public async Task<IActionResult> All(string id)
        {
            if (id == null)
            {
                id = this._userManager.GetUserId(this.User);
                this.ViewData["layout"] = ACCOUNT_LAYOUT;
            }
            else
            {
                this.ViewData["layout"] = TEACHER_LAYOUT;
            }

            return View(new ReviewAllListViewModel
            {
                Reviews = await this._reviewsService.GetAllByReceiverAsync<ReviewAllViewModel>(id)
            });
        }
    }
}