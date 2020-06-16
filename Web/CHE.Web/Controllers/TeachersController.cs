namespace CHE.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using CHE.Services.Data;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Teachers;

    public class TeachersController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 18;

        private readonly ITeachersService _teachersService;

        public TeachersController(ITeachersService teachersService)
        {
            this._teachersService = teachersService;
        }

        public async Task<IActionResult> All(int pageIndex = 1)
        {
            var teachers = this._teachersService
                    .GetAll<TeacherAllViewModel>();

            var teachersList = await PaginatedList<TeacherAllViewModel>
                 .CreateAsync(teachers, pageIndex, DEFAULT_PAGE_SIZE);

            return View(teachersList);
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentTeacher = await this._teachersService
                .GetByIdAsync<TeacherDetailsViewModel>(id);
            this.ViewData["id"] = currentTeacher.Id;

            return this.View(currentTeacher);
        }
    }
}