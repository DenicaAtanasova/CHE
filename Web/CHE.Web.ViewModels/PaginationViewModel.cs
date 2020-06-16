namespace CHE.Web.ViewModels
{
    public class PaginationViewModel
    {
        public int PageIndex { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }
    }
}