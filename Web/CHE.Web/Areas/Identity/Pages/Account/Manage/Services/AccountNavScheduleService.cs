namespace CHE.Web.Areas.Identity.Pages.Account.Manage.Services
{
    using CHE.Data.Models;
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Identity;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AccountNavScheduleService : IAccountNavScheduleService
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ISchedulesService _schedulesService;

        public AccountNavScheduleService(UserManager<CheUser> userManager, ISchedulesService schedulesService)
        {
            this._userManager = userManager;
            this._schedulesService = schedulesService;
        }

        public async Task<string> GetScheduleIdByUserAsync(ClaimsPrincipal user)
        {
            var userId = this._userManager.GetUserId(user);

            return await this._schedulesService.GetIdByUserAsync(userId);
        }
    }

    public interface IAccountNavScheduleService
    {
        Task<string> GetScheduleIdByUserAsync(ClaimsPrincipal user);
    }
}