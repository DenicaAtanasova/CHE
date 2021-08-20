namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;
    using System.ComponentModel.DataAnnotations;

    public class Message : BaseModel<string>
    {
        [Required]
        public string Sender { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string MessengerId { get; set; }

        public Messenger Messenger { get; set; }
    }
}