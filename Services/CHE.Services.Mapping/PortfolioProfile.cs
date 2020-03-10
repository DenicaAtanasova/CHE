namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.InputModels.Portfolios;
    using CHE.Web.ViewModels.Teachers;

    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            this.CreateMap<Portfolio, PortfolioInputModel>()
                .ForMember(dest => dest.Image, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            this.CreateMap<Portfolio, TeacherPortfolioDetailsViewModel>();
        }
    }
}