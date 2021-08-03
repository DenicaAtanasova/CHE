namespace CHE.Data.Models
{
    public class ParentCooperative
    {
        public string ParentId { get; init; }

        public Parent Parent { get; init; }

        public string CooperativeId { get; init; }

        public Cooperative Cooperative { get; init; }
    }
}