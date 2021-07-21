namespace CHE.Web.ViewModels.Reviews
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class ReviewAllViewModel : IMapFrom<Review>
    {
        public string Id { get; init; }

        public string Comment { get; init; }

        public int Rating { get; init; }

        public string SenderUserName { get; init; }
    }
}