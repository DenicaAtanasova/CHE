namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class GradesListViewComponent : ViewComponent
    {
        private readonly IGradesService _gradesService;

        public GradesListViewComponent(IGradesService gradesService)
        {
            _gradesService = gradesService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var gradesList = await _gradesService.GetAllAsync();

            return View(gradesList);
        }
    }
}
