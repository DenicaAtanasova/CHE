namespace CHE.Web.ViewModels.Reviews
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class ReviewAllViewModel : IMapFrom<Review>
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public string SenderUserName { get; set; }

        public string ReceiverId { get; set; }
    }
}