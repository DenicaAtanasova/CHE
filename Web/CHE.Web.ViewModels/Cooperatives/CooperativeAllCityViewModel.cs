namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeAllCityViewModel : IMapFrom<Address>
    {
        public string City { get; set; }
    }
}