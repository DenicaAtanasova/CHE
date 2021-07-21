namespace CHE.Data.Models
{
    public class CheUserCooperative
    {
        public string CheUserId { get; init; }

        public CheUser CheUser { get; init; }

        public string CooperativeId { get; init; }

        public Cooperative Cooperative { get; init; }
    }
}