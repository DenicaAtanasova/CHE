namespace CHE.Web.ViewModels.Teachers
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class TeacherDetailsViewModel : IMapFrom<Teacher>
    {
        public string Id { get; init; }

        public string SentReviewId { get; set; }

        public TeacherProfileDetailsViewModel Profile { get; init; }
    }
}