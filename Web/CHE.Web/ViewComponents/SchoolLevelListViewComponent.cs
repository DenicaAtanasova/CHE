namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;
    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Threading.Tasks;

    public class SchoolLevelListViewComponent : ViewComponent
    {
        private readonly IPortfoliosService _portfoliosService;

        public SchoolLevelListViewComponent(IPortfoliosService portfoliosService)
        {
            this._portfoliosService = portfoliosService;
        }

        public IViewComponentResult Invoke(string currentSchoolLevel)
        {
            var schoolLevelList = this._portfoliosService.GetAllSchoolLevels(currentSchoolLevel);

            return this.View(schoolLevelList);
        }
    }
}
