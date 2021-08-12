namespace CHE.Web.Controllers
{
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Mvc;

    using System.Linq;
    using System.Threading.Tasks;

    using static WebConstants;

    public class TeachersController : Controller
    {
        private readonly ITeachersService _teachersService;
        private readonly IReviewsService _reviewsService;

        public TeachersController(
            ITeachersService teachersService,
            IReviewsService reviewsService)
        {
            this._teachersService = teachersService;
            this._reviewsService = reviewsService;
        }

        public async Task<IActionResult> All(string level, string city, string neighbourhood, int pageIndex = 1)
        {
            var teachers = await this._teachersService.GetAllAsync<TeacherAllViewModel>(
                pageIndex, 
                DefaultPageSize,
                level,
                city,
                neighbourhood);

            var count = 0;
            if (teachers != null && teachers.Count() >= DefaultPageSize)
            {
                count = await this._teachersService
                    .CountAsync(level, city, neighbourhood);
            }            

            TempData["levelDisplayName"] = "school level";

            var teachersList = new TeacherAllListViewModel
            {
                Teachers = PaginatedList<TeacherAllViewModel>
                 .Create(teachers, count, pageIndex, DefaultPageSize),
                Filter = new FilterViewModel
                {
                    Level = level,
                    City = city,
                    Neighbourhood = neighbourhood
                }
            };

            return View(teachersList);
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentTeacher = await this._teachersService
                .GetByIdAsync<TeacherDetailsViewModel>(id);

            if (currentTeacher == null)
            {
                return this.NotFound();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.GetId();
                currentTeacher.SentReviewId = await this._reviewsService
                    .GetSentReviewIdAsync(userId, id);
            }
            
            this.ViewData["id"] = currentTeacher.Id;

            return this.View(currentTeacher);
        }
    }
}