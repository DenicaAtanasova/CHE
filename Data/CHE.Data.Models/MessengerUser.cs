namespace CHE.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MessengerUser
    {
        [Required]
        public string MessengerId { get; set; }

        public Messenger Messenger { get; set; }

        [Required]
        public string UserId { get; set; }

        public CheUser User { get; set; }
    }
}