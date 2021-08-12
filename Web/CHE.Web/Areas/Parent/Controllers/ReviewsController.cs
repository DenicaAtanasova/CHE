namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Common.Extensions;
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.InputModels.Reviews;

    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class ReviewsController : ParentController
    {
        private readonly IParentsService _parentsService;
        private readonly IReviewsService _reviewsService;

        public ReviewsController(
            IParentsService parentsService,
            IReviewsService reviewsService)
        {
            this._parentsService = parentsService;
            this._reviewsService = reviewsService;
        }

        public IActionResult Send(string id)
        {
            if (!id.IsValidString())
            {
                return this.NotFound();
            }

            return this.View(new ReviewCreateInputModel { ReceiverId = id});
        }

        [HttpPost]
        public async Task<IActionResult> Send(ReviewCreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var userId = this.User.GetId();
            await this._parentsService.SendReviewAsync(
                userId, 
                inputModel.ReceiverId, 
                inputModel.Comment, 
                inputModel.Rating);

            return this.RedirectToAction("All", "Reviews", new { area = "", id = inputModel.ReceiverId });
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
                return this.View(inputModel);
            }

            await this._reviewsService.UpdateAsync(inputModel.Id, inputModel.Comment, inputModel.Rating);
            
            return RedirectToAction("All", "Reviews", new { area = "", id = inputModel.ReceiverId });
        }

        public async Task<IActionResult> Delete(string reviewId, string receiverId)
        {
            await this._reviewsService.DeleteAsync(reviewId);
            return RedirectToAction("Details", "Teachers", new { area = "", id = receiverId });
        }
    }
}