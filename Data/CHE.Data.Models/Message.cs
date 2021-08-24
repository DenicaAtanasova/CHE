namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    using static CHE.Common.DataConstants.Message;

    public class Message : BaseModel<string>
    {
        [Required]
        public string Sender { get; set; }

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; }

        [Required]
        public string MessengerId { get; set; }

        public Messenger Messenger { get; set; }
    }
}