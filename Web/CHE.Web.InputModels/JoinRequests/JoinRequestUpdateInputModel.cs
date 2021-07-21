namespace CHE.Web.InputModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class JoinRequestUpdateInputModel : IMapFrom<JoinRequest>
    {
        public string Id { get; init; }

        [Required]
        public string Content { get; init; }

        [Required]
        public string CooperativeId { get; init; }

        public string CooperativeName { get; init; }
    }
}