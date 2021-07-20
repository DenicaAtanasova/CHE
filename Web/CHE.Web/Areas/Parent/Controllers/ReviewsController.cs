namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.InputModels.Reviews;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ReviewsController : ParentController
    {
        private readonly ICheUsersService _cheUsersService;
        private readonly IReviewsService _reviewsService;

        public ReviewsController(
            ICheUsersService cheUsersService,
            IReviewsService reviewsService)
        {
            this._cheUsersService = cheUsersService;
            this._reviewsService = reviewsService;
        }

        public IActionResult Send(string teacherId) =>
            this.View(new ReviewCreateInputModel { ReceiverId = teacherId });

        [HttpPost]
        public async Task<IActionResult> Send(ReviewCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                //TODO: check why this is not working
                //return this.View(inputModel.ReceiverId);
                return this.RedirectToAction(nameof(Send), new { id = inputModel.ReceiverId });
            }

            var senderId = this.User.GetId();
            await _cheUsersService.SendReviewAsync(senderId, inputModel);

            return this.RedirectToAction("All", "Teachers", new { area = "" });
        }

        public async Task<IActionResult> Update(string id)
        {
            var review = await this._reviewsService
                .GetByIdAsync<ReviewUpdateInputModel>(id);

            if (review == null)
            {
                return this.NotFound();
            }

            return this.View(review);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReviewUpdateInputModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(inputModel.Id);
            }

            await this._reviewsService.UpdateAsync(inputModel);

            return RedirectToAction("Details", "Teachers", new { area = "", id = inputModel.ReceiverId });
        }

        public async Task<IActionResult> Delete(string reviewId, string receiverId)
        {
            await this._reviewsService.DeleteAsync(reviewId);
            return RedirectToAction("Details", "Teachers", new { area = "", id = receiverId });
        }
    }
}