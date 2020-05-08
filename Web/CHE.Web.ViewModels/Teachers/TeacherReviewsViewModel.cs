namespace CHE.Web.ViewModels.Teachers
{
    using CHE.Web.ViewModels.Reviews;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.Collections.Generic;

    public class TeacherReviewsViewModel : IMapFrom<CheUser>
    {
        public string Id { get; set; }

        public IEnumerable<ReviewAllViewModel> Reviews { get; set; }
    }
}