namespace CHE.Web.Tests.Data
{
    using CHE.Web.InputModels.Reviews;
    using CHE.Web.ViewModels.Reviews;

    using System.Collections.Generic;

    public class Reviews
    {
        public static IEnumerable<ReviewAllViewModel> AllRceiverReviews =>
            new List<ReviewAllViewModel>
            {
                new ReviewAllViewModel(),
                    new ReviewAllViewModel(),
                    new ReviewAllViewModel(),
            };

        public static ReviewUpdateInputModel ReviewToUpdate =>
            new ReviewUpdateInputModel
            {
                Id = "id",
                Comment = "coment",
                Rating = 4,
                ReceiverId = "review id"
            };
    }
}