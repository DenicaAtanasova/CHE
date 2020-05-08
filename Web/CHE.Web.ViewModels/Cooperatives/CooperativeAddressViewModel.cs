namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeAddressViewModel : IMapFrom<Address>
    {
        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public string Street { get; set; }
    }
}