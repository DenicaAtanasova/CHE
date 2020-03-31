namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.ViewModels.Reviews;

    public class ReveiwProfile : Profile
    {
        public ReveiwProfile()
        {
            this.CreateMap<Review, ReviewAllViewModel>();
        }
    }
}