namespace CHE.Web.InputModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    public class JoinRequestCreateInputModel : IMapTo<JoinRequest>
    {
        [Required]
        public string Content { get; set; }

        public string ReceiverId { get; set; }

        [Required]
        public string CooperativeId { get; set; }
    }
}