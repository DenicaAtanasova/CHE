namespace CHE.Web.Controllers
{
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using System.Threading.Tasks;

    public class TeachersController : Controller
    {
        private const int DEFAULT_PAGE_SIZE = 6;

        private readonly ICheUsersService _cheUsersService;
        private readonly IReviewsService _reviewsService;

        public TeachersController(
            ICheUsersService cheUsersService,
            IReviewsService reviewsService)
        {
            this._cheUsersService = cheUsersService;
            this._reviewsService = reviewsService;
        }

        public async Task<IActionResult> All(TeacherAllFilterViewModel filter, int pageIndex = 1)
        {
            var teachers = await this._cheUsersService
                    .GetAllAsync<TeacherAllViewModel>(pageIndex, DEFAULT_PAGE_SIZE, filter.SchoolLevel);

            var count = await this._cheUsersService.CountAsync(filter.SchoolLevel);

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
            var currentTeacher = await this._cheUsersService
                .GetByIdAsync<TeacherDetailsViewModel>(id);

            if (currentTeacher == null)
            {
                return this.NotFound();
            }

            var userId = this.User.GetId();
            currentTeacher.SentReviewId = await this._reviewsService.GetSentReviewIdAsync(id, userId);
            this.ViewData["id"] = currentTeacher.Id;

            return this.View(currentTeacher);
        }
    }
}