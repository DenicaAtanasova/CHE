namespace CHE.Services.Data
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using AutoMapper;
    using CHE.Data.Models;
    using CHE.Data;

    public class UsersService : IUsersService
    {
        private readonly CheDbContext _dbContext;
        private readonly UserManager<CheUser> _userManager;
        private readonly IJoinRequestsService _joinRequestsService;

        public UsersService(CheDbContext dbContext, UserManager<CheUser> userManager, IJoinRequestsService joinRequestsService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._joinRequestsService = joinRequestsService;
        }

        public async Task<bool> AcceptRequest(string requestId)
        {
            var request = await this._joinRequestsService.GetByIdAsync<JoinRequest>(requestId);
            var sender = await this._userManager.FindByNameAsync(request.Sender.UserName);

            //TODO UserCooperatives keys might be duplicated
            sender.Cooperatives.Add(new CheUserCooperative { CooperativeId = request.CooperativeId});

            var result = await this._dbContext.SaveChangesAsync() > 0;
            await this._joinRequestsService.DeleteAsync(requestId);

            return result;
        }

        public async Task<bool> RejectRequest(string requestId)
        {
            var result = await this._joinRequestsService.DeleteAsync(requestId);

            return result;
        }
    }
}