namespace CHE.Services.Mapping
{
    using CHE.Data.Models;

    using CHE.Web.ViewModels.Teachers;
    using AutoMapper;

    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            this.CreateMap<CheUser, TeacherAllViewModel>();

            this.CreateMap<CheUser, TeacherDetailsViewModel>();
        }
    }
}