namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels.Teachers;

    public class TeachersController : Controller
    {
        private readonly ITeachersService _teachersService;

        public TeachersController(ITeachersService teachersService)
        {
            this._teachersService = teachersService;
        }

        public async Task<IActionResult> All()
        {
            var teachers = new TeacherAllListViewModel
            {
                Teachers = await this._teachersService.GetAllAsync<TeacherAllViewModel>()
            };

            return View(teachers);
        }
    }
}