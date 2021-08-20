namespace CHE.Data.Models
{
    using Common.Models;

    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.JoinRequst;

    public class JoinRequest : BaseModel<string>
    {
        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string SenderId { get; set; }

        public Parent Sender { get; set; }

        [Required]
        public string CooperativeId { get; init; }

        public Cooperative Cooperative { get; init; }
    }
}