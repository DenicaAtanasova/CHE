namespace CHE.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ParentCooperative
    {
        [Required]
        public string ParentId { get; init; }

        public Parent Parent { get; init; }

        [Required]
        public string CooperativeId { get; init; }

        public Cooperative Cooperative { get; init; }
    }
}