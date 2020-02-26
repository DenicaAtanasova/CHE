namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using CHE.Data.Models;

    public class UsersService : IUsersService
    {
        private readonly UserManager<CheUser> _userManager;
        private readonly IJoinRequestsService _joinRequestsService;
        private readonly ICooperativesService _cooperativesService;

        public UsersService(
            UserManager<CheUser> userManager, 
            IJoinRequestsService joinRequestsService,
            ICooperativesService cooperativesService)
        {
            this._userManager = userManager;
            this._joinRequestsService = joinRequestsService;
            this._cooperativesService = cooperativesService;
        }

        public async Task<bool> SendJoinRequest(string requestContent, string cooperativeId, string receiverId, string senderName)
        {
            var sender = await this._userManager.FindByNameAsync(senderName);

            var result = await this._joinRequestsService
                .CreateAsync(requestContent, cooperativeId, sender.Id, receiverId);

            return result;
        }

        public async Task<bool> AcceptRequest(string requestId)
        {
            var request = await this._joinRequestsService.GetByIdAsync<JoinRequest>(requestId);

            var memberAddSucceeded = await this._cooperativesService.AddMemberAsync(request.CooperativeId, request.SenderId);
            var requestDeleteSucceeded = await this._joinRequestsService.DeleteAsync(requestId);

            return requestDeleteSucceeded && memberAddSucceeded;
        }

        public async Task<bool> RejectRequest(string requestId)
        {
            var result = await this._joinRequestsService.DeleteAsync(requestId);

            return result;
        }
    }
}