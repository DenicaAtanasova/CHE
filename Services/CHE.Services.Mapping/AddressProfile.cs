namespace CHE.Services.Mapping
{
    using AutoMapper;

    using CHE.Data.Models;
    using CHE.Web.InputModels.Cooperatives;
    using CHE.Web.ViewModels.Cooperatives;

    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            this.CreateMap<Address, CooperativeAddressInputModel>().ReverseMap();
            this.CreateMap<Address, CooperativeAddressViewModel>();
        }
    }
}