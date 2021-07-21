namespace CHE.Web.InputModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    using static DataConstants.JoinRequst;
    using static DataErrorMessages;

    public class JoinRequestCreateInputModel : IMapTo<JoinRequest>
    {
        [Required]
        [StringLength(
            ContentMaxLength,
            MinimumLength = ContentMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Content { get; init; }

        public string ReceiverId { get; init; }

        [Required]
        public string CooperativeId { get; init; }
    }
}