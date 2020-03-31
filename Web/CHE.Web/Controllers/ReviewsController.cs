namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Reviews;
    using CHE.Web.ViewModels.Teachers;

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
                Id = id,
                Reviews = await this._reviewsService.GetAllAsync<ReviewAllViewModel>(id)
            };

            return View(reviewsList);
        }
    }
}