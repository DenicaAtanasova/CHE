namespace CHE.Web.ViewComponents
{
    using CHE.Web.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string controller, int pageIndex, bool hasPreviousPage, bool hasNextPage, IFilter filter)
        {
            var paginationModel = new PaginationViewModel 
            {
                Controller = controller,
                PageIndex = pageIndex,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
                Filter = filter
            };

            return this.View(paginationModel);
        }
    }
}
