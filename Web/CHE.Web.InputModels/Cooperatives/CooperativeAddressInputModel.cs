namespace CHE.Web.InputModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeAddressInputModel : IMapFrom<Address>, IMapTo<Address>
    {
        public string City { get; set; }

        public string Neighbourhood { get; set; }

        public string Street { get; set; }
    }
}