namespace CHE.Data.Models
{
    using Common.Models;

    public class Review : BaseDeletableModel<string>
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public string SenderId { get; set; }

        public Parent Sender { get; set; }

        public string RecieverId { get; set; }

        public Teacher Receiver { get; set; }
    }
}