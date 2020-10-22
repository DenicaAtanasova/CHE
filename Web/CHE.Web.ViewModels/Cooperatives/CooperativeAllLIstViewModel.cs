namespace CHE.Web.ViewModels.Cooperatives
{
    public class CooperativeAllLIstViewModel
    {
        public PaginatedList<CooperativeAllViewModel> Cooperatives { get; set; }

        public CooperativeAllFilterViewModel Filter { get; set; }
    }
}