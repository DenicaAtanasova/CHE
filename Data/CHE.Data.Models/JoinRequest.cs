namespace CHE.Data.Models
{
    using Common.Models;

    public class JoinRequest : BaseDeletableModel<string>
    {
        public string Content { get; set; }

        public string SenderId { get; set; }

        public CheUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public CheUser Receiver { get; set; }

        public string CooperativeId { get; set; }

        public Cooperative Cooperative { get; set; }
    }
}