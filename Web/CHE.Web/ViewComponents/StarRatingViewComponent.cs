namespace CHE.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using CHE.Web.ViewModels;

    public class StarRatingViewComponent : ViewComponent
    {
        private const int TOTAL_STAR_COUNT = 5;

        public IViewComponentResult Invoke(double rating)
        {
            var fullStarCount = (int)rating;
            var halfStarCount = rating % 1 != 0 ? 1 : 0;
            var emptyStarCount = TOTAL_STAR_COUNT - (fullStarCount + halfStarCount);

            var starRatingModel = new StarRatingViewModel
            {
                FullStarCount = fullStarCount,
                HalfStarCount = halfStarCount,
                EmptyStarCount = emptyStarCount
            };

            return this.View(starRatingModel);
        }
    }
}