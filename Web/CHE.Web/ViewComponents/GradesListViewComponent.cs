namespace CHE.Web.ViewComponents
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using CHE.Services.Data;

    public class GradesListViewComponent : ViewComponent
    {
        private readonly IGradesService _gradesService;

        public GradesListViewComponent(IGradesService gradesService)
        {
            _gradesService = gradesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string currentGrade = null)
        {
            var gradesList = await _gradesService.GetAllValuesAsync(currentGrade);

            return this.View(gradesList);
        }
    }
}
