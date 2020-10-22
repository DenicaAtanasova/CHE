namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class CooperativeAllNeighbourhoodViewModel : IMapFrom<Address>
    {
        public string Neighbourhood { get; set; }
    }
}