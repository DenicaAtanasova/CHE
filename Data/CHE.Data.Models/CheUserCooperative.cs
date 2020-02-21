namespace CHE.Data.Models
{
    public class CheUserCooperative
    {
        public string CheUserId { get; set; }

        public CheUser CheUser { get; set; }

        public string CooperativeId { get; set; }

        public Cooperative Cooperative { get; set; }
    }
}