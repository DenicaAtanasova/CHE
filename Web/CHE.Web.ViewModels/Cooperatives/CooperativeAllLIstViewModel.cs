using System.Collections.Generic;

namespace CHE.Web.ViewModels.Cooperatives
{
    public class CooperativeAllListViewModel
    {
        public PaginatedList<CooperativeAllViewModel> Cooperatives { get; set; }

        public IEnumerable<string> Grades { get; set; }

        public IEnumerable<string> Cities { get; set; }

        public IEnumerable<string> Neighbourhoods { get; set; }

        public CooperativeAllFilterViewModel Filter { get; set; }
    }
}