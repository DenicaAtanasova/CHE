namespace CHE.Web.ViewModels
{
    public class PaginationViewModel
    {
        public string Controller { get; set; }

        public int PageIndex { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public IFilter Filter { get; set; }
    }
}