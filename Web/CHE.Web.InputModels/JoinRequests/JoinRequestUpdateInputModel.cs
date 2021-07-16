namespace CHE.Web.InputModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class JoinRequestUpdateInputModel : IMapFrom<JoinRequest>
    {
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CooperativeId { get; set; }

        public string CooperativeName { get; set; }
    }
}