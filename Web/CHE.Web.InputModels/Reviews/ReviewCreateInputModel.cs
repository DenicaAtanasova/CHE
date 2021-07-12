namespace CHE.Web.InputModels.Reviews
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class ReviewCreateInputModel : IMapTo<Review>
    {
        private const string RATING_ERR_MSG = "Rating must be between 1 and 5!";
        private const string COMMENT_ERR_MSG = "You must leave a comment!";

        [Required(ErrorMessage = COMMENT_ERR_MSG)]
        public string Comment { get; set; }

        [Required]
        [Range(1,5, ErrorMessage = RATING_ERR_MSG)]
        public int Rating { get; set; }

        public string ReceiverId { get; set; }
    }
}