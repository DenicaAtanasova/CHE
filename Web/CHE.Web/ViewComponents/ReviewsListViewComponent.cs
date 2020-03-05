namespace CHE.Web.ViewComponents
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Teachers;

    public class ReviewsListViewComponent : ViewComponent
    {
        private readonly IReviewsService _reviewsService;

        public ReviewsListViewComponent(IReviewsService reviewsService)
        {
            this._reviewsService = reviewsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string teacherId)
        {
            var reviews = await this._reviewsService
                .GetTeachersAllAsync<TeacherReviewDetailsViewModel>(teacherId);

            return this.View(reviews);
        }
    }
}
