﻿namespace CHE.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using CHE.Data.Models;
    using CHE.Data;


    public class UsersService : IUsersService
    {
        private readonly CheDbContext _dbContext;
        private readonly UserManager<CheUser> _userManager;
        private readonly IJoinRequestsService _joinRequestsService;

        public UsersService(
            CheDbContext dbContext,
            UserManager<CheUser> userManager, 
            IJoinRequestsService joinRequestsService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._joinRequestsService = joinRequestsService;
        }

        public async Task<bool> SendJoinRequestAsync(string requestContent, string cooperativeId, string receiverId, string senderName)
        {
            var sender = await this._userManager.FindByNameAsync(senderName);

            var result = await this._joinRequestsService
                .CreateAsync(requestContent, cooperativeId, sender.Id, receiverId);

            return result;
        }

        public async Task<bool> AcceptRequestAsync(string requestId)
        {
            var request = await this._joinRequestsService.GetByIdAsync<JoinRequest>(requestId);

            var memberAddSucceeded = await this.AddMemberToCooperativeAsync(request.SenderId, request.CooperativeId);
            var requestDeleteSucceeded = await this._joinRequestsService.DeleteAsync(requestId);

            return requestDeleteSucceeded && memberAddSucceeded;
        }

        public async Task<bool> RejectRequestAsync(string requestId)
        {
            var result = await this._joinRequestsService.DeleteAsync(requestId);

            return result;
        }

        public async Task<bool> RemoveMemberFromCooperativeAsync(string memberId, string cooperativeId)
        {
            var cooperativeMember = await this._dbContext.UserCooperatives
                .SingleOrDefaultAsync(x => x.CheUserId == memberId && x.CooperativeId == cooperativeId);

            this._dbContext.UserCooperatives.Remove(cooperativeMember);

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        private async Task<bool> AddMemberToCooperativeAsync(string senderId, string cooperativeId)
        {
            var cooperative = await this._dbContext.Cooperatives
                .SingleOrDefaultAsync(x => x.Id == cooperativeId);

            cooperative.Members.Add(
                new CheUserCooperative
                {
                    CheUserId = senderId
                }
            );
            var isModified = this._dbContext.Update(cooperative).State == EntityState.Modified;

            return isModified;
        }
    }
}