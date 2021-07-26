namespace CHE.Web.InputModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Review;
    using static DataErrorMessages;

    public class ReviewCreateInputModel
    {
        [Required]
        [StringLength(
            CommentMaxLength,
            MinimumLength = CommentMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Comment { get; init; }

        [Required]
        [Range(RatingMinValue, RatingMaxValue, ErrorMessage = RangeErroMessage)]
        public int Rating { get; init; }

        public string ReceiverId { get; init; }
    }
}