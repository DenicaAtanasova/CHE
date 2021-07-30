namespace CHE.Web.InputModels.JoinRequests
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.JoinRequst;
    using static DataErrorMessages;

    public class JoinRequestCreateInputModel
    {
        [Required]
        [StringLength(
            ContentMaxLength,
            MinimumLength = ContentMinLength,
            ErrorMessage = StringLengthErroMessage)]
        public string Content { get; init; }

        [Required]
        public string CooperativeId { get; init; }
    }
}