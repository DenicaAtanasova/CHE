namespace CHE.Web.ViewComponents
{
    using CHE.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class StarRatingViewComponent : ViewComponent
    {
        private const int TotalStarCount = 5;

        public IViewComponentResult Invoke(double rating)
        {
            var fullStarCount = (int)rating;
            var halfStarCount = rating % 1 != 0 ? 1 : 0;
            var emptyStarCount = TotalStarCount - (fullStarCount + halfStarCount);

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