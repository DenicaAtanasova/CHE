namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Reviews;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;

        public ReviewsController(IReviewsService reviewsService)
        {
            this._reviewsService = reviewsService;
        } 

        public async Task<IActionResult> All(string id)
        {
            var reviewsList = new TeacherReviewsViewModel
            {
                Reviews = await this._reviewsService.GetAllByReceiverAsync<ReviewAllViewModel>(id)
            };

            this.ViewData["id"] = id;

            return View(reviewsList);
        }
    }
}