namespace CHE.Web.InputModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using System.ComponentModel.DataAnnotations;

    using static DataConstants.JoinRequst;
    using static DataErrorMessages;

    public class JoinRequestUpdateInputModel : IMapFrom<JoinRequest>
    {
        [Required]
        public string Id { get; init; }

        [Required]
        [StringLength(
            ContentMaxLength,
            MinimumLength = ContentMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Content { get; init; }

        [Required]
        public string CooperativeId { get; init; }

        public string CooperativeName { get; init; }
    }
}