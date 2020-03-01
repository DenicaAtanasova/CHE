using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CHE.Data.Models;
using CHE.Services.Data;
using CHE.Web.ViewModels.Teachers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    public class JoinRequestModel : PageModel
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly IJoinRequestsService _joinRequestsService;

        public JoinRequestModel(
            UserManager<CheUser> userManager,
            IJoinRequestsService joinRequestsService
            )
        {
            this._userManager = userManager;
            this._joinRequestsService = joinRequestsService;
        }

        [BindProperty]
        public IEnumerable<TeacherJoinRequestVIewModel> Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this._userManager.GetUserId(this.User);
            this.Input = await this._joinRequestsService
                .GetTeacherAllAsync<TeacherJoinRequestVIewModel>(userId);

            if (this.Input == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
