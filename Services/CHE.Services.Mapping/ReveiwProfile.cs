namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Teachers;

    public class ReveiwProfile : Profile
    {
        public ReveiwProfile()
        {
            this.CreateMap<Review, TeacherReviewDetailsViewModel>();
        }
    }
}