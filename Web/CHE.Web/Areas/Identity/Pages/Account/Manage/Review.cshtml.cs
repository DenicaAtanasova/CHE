namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Reviews;

    using System.Collections.Generic;

    public class ReviewModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly IReviewsService _reviewsService;

        public ReviewModel(UserManager<CheUser> userManager, IReviewsService reviewsService)
        {
            this._userManager = userManager;
            this._reviewsService = reviewsService;
            this.Reviews = new List<ReviewAllViewModel>();
        }

        public IEnumerable<ReviewAllViewModel> Reviews { get; set; }

        public void OnGet()
        {
            var teacherId = this._userManager.GetUserId(this.User);

            this.Reviews = this._reviewsService
                .GetAllAsync<ReviewAllViewModel>(teacherId).GetAwaiter().GetResult();
        }
    }
}