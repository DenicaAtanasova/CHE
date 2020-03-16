namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using CHE.Data.Models;

    public class ReviewModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;

        public ReviewModel(UserManager<CheUser> userManager)
        {
            this._userManager = userManager;
            this.Teacher = new TeacherViewModel();
        }

        public TeacherViewModel Teacher { get; set; }

        public class TeacherViewModel
        {
            public string Id { get; set; }
        }

        public void OnGet()
        {
            this.Teacher.Id = this._userManager.GetUserId(this.User); ;
        }
    }
}