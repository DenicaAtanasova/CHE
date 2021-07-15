namespace CHE.Web.ViewComponents
{
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Mvc;

    public class SchoolLevelListViewComponent : ViewComponent
    {
        private readonly IProfilesService _profilesService;

        public SchoolLevelListViewComponent(IProfilesService profilesService)
        {
            this._profilesService = profilesService;
        }

        public IViewComponentResult Invoke(string currentSchoolLevel)
        {
            var schoolLevelList = this._profilesService.GetAllSchoolLevels(currentSchoolLevel);

            return this.View(schoolLevelList);
        }
    }
}
