namespace CHE.Web.ViewModels.Cooperatives
{
    using CHE.Web.ViewModels;

    public class CooperativesAllListViewModel
    {
        public PaginatedList<CooperativeAllViewModel> Cooperatives { get; set; }

        public FilterViewModel Filter { get; set; }
    }
}