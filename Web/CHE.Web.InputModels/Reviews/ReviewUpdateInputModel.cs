namespace CHE.Web.InputModels.Reviews
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Review;
    using static DataErrorMessages;

    public class ReviewUpdateInputModel : IMapFrom<Review>
    {
        [Required]
        public string Id { get; init; }

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

        public string ReceiverUserUserName { get; init; }
    }
}