namespace CHE.Web.Services
{
    using CHE.Data.Models;
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Identity;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AccountNavScheduleService
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ISchedulesService _schedulesService;

        public AccountNavScheduleService(UserManager<CheUser> userManager, ISchedulesService schedulesService)
        {
            _userManager = userManager;
            _schedulesService = schedulesService;
        }

        public async Task<string> GetScheduleIdByUserAsync(ClaimsPrincipal user)
        {
            var userId = _userManager.GetUserId(user);

            return await _schedulesService.GetIdByUserAsync(userId);
        }
    }
}