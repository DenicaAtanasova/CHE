namespace CHE.Web.ViewModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestTeacherAllVIewModel : IMapFrom<JoinRequest>
    {
        public string Id { get; set; }

        public string CooperativeName { get; set; }
    }
}