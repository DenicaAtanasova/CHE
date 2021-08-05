namespace CHE.Data.Models
{
    public class MessengerUser
    {
        public string MessengerId { get; set; }

        public Messenger Messenger { get; set; }

        public string UserId { get; set; }

        public CheUser User { get; set; }
    }
}