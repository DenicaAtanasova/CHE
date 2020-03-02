using System.ComponentModel.DataAnnotations;

namespace CHE.Web.InputModels.JoinRequests
{
    public class JoinRequestCreateInputModel
    {
        [Required]
        public string Content { get; set; }

        public string ReceiverId { get; set; }

        [Required]
        public string CooperativeId { get; set; }
    }
}