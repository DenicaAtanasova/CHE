namespace CHE.Web.ViewComponents
{
    using CHE.Data.Models.Enums;
    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Linq;

    public class GradesListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string currentGrade = null)
        {
            var gradesList = Enum.GetValues<Grade>()
                .Cast<Grade>()
                .Where(x => x.ToString() != currentGrade)
                .Select(x => x.ToString());

            return this.View(gradesList);
        }
    }
}