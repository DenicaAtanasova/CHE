namespace CHE.Web.InputModels.Reviews
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class ReviewUpdateInputModel : IMapFrom<Review>
    {
        private const string RATING_ERR_MSG = "Rating must be between 1 and 5!";
        private const string COMMENT_ERR_MSG = "You must leave a comment!";

        public string Id { get; init; }

        [Required(ErrorMessage = COMMENT_ERR_MSG)]
        public string Comment { get; init; }

        [Required]
        [Range(1,5, ErrorMessage = RATING_ERR_MSG)]
        public int Rating { get; init; }

        public string ReceiverId { get; init; }

        public string ReceiverUserName { get; init; }
    }
}