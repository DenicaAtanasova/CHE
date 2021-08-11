namespace CHE.Web.Tests.Data
{
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
    }
}