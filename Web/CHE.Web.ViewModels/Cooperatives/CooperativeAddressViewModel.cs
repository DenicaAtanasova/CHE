namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeAddressViewModel : IMapFrom<Address>
    {
        public string City { get; init; }

        public string Neighbourhood { get; init; }

        public string Street { get; init; }
    }
}