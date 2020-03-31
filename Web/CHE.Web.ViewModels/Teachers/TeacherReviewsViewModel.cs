using CHE.Web.ViewModels.Reviews;

namespace CHE.Web.ViewModels.Teachers
{
    using System.Collections.Generic;

    public class TeacherReviewsViewModel
    {
        public string Id { get; set; }

        public IEnumerable<ReviewAllViewModel> Reviews { get; set; }
    }
}