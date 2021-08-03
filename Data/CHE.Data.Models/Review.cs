namespace CHE.Data.Models
{
    using Common.Models;

    public class Review : BaseModel<string>
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public string SenderId { get; set; }

        public Parent Sender { get; set; }

        public string ReceiverId { get; init; }

        public Teacher Receiver { get; init; }
    }
}