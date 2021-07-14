namespace CHE.Web.InputModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System;
    using System.ComponentModel.DataAnnotations;

    public class JoinRequestUpdateInputModel : IMapFrom<JoinRequest>, IMapTo<JoinRequest>
    {
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string CooperativeId { get; set; }

        [Required]
        public string CooperativeName { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string SenderId { get; set; }
    }
}