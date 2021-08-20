namespace CHE.Data.Models
{
    using Common.Models;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Review;

    public class Review : BaseModel<string>
    {
        [Required]
        [MaxLength(CommentMaxLength)]
        public string Comment { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string SenderId { get; set; }

        public Parent Sender { get; set; }

        [Required]
        public string ReceiverId { get; init; }

        public Teacher Receiver { get; init; }
    }
}