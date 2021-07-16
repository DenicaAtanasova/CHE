namespace CHE.Web.Services
{
    using CHE.Data.Models;
    using CHE.Services.Data;

    using Microsoft.AspNetCore.Identity;

    using System.Security.Claims;
    using System.Threading.Tasks;

    public class CooperativeLayoutService
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly ISchedulesService _schedulesService;
        private readonly ICooperativesService _cooperativesService;

        public CooperativeLayoutService(
            UserManager<CheUser> userManager, 
            ISchedulesService schedulesService,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._schedulesService = schedulesService;
            this._cooperativesService = cooperativesService;
        }

        public async Task<string> GetScheduleIdByCooperativeAsync(string cooperativeId) =>
            await this._schedulesService.GetIdByCooperativeAsync(cooperativeId);

        public async Task<bool> IsMemberAsync(ClaimsPrincipal user, string cooperativeId)
        {
            var userId = this._userManager.GetUserId(user);

             return await this._cooperativesService.CheckIfMemberAsync(userId, cooperativeId);
        }

        public async Task<bool> IsAdminAsync(ClaimsPrincipal user, string cooperativeId)
        {
            var userId = this._userManager.GetUserId(user);

            return await this._cooperativesService.CheckIfAdminAsync(userId, cooperativeId);
        }
    }
}