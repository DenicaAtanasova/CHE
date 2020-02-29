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
                .ReverseMap();

            this.CreateMap<Portfolio, TeacherPortfolioDetailsViewModel>();
        }
    }
}