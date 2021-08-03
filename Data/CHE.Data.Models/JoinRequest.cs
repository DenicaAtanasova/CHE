namespace CHE.Data.Models
{
    using Common.Models;

    public class JoinRequest : BaseModel<string>
    {
        public string Content { get; set; }

        public string SenderId { get; set; }

        public Parent Sender { get; set; }

        public string CooperativeId { get; init; }

        public Cooperative Cooperative { get; init; }
    }
}