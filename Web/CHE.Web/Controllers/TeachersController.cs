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

        public async Task<IActionResult> All(TeacherAllFilterViewModel filter, int pageIndex = 1)
        {
            var teachers = await this._teachersService
                    .GetAllAsync<TeacherAllViewModel>(pageIndex, DEFAULT_PAGE_SIZE, filter.SchoolLevel);

            var count = await this._teachersService.Count(filter.SchoolLevel);

            var teachersList = new TeacherAllListViewModel
            {
                Teachers = PaginatedList<TeacherAllViewModel>
                 .Create(teachers, count, pageIndex, DEFAULT_PAGE_SIZE),
                Filter = filter
            };

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