namespace CHE.Web.ViewModels.Teachers
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class TeacherJoinRequestVIewModel : IMapFrom<JoinRequest>
    {
        public string Id { get; set; }

        public string CooperativeName { get; set; }
    }
}