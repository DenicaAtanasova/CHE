namespace CHE.Data.Models
{
    using CHE.Data.Common.Models;

    public class Message : BaseModel<string>
    {
        public string Sender { get; set; }

        public string Text { get; set; }

        public string MessengerId { get; set; }

        public Messenger Messenger { get; set; }
    }
}