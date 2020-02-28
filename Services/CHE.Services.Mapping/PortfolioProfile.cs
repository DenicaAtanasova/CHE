namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.InputModels.Portfolios;

    public class PortfolioProfile : Profile
    {
        public PortfolioProfile()
        {
            this.CreateMap<Portfolio, PortfolioInputModel>()
                .ReverseMap();
        }
    }
}