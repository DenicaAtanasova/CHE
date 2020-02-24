namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CHE.Data;
    using CHE.Data.Models;
    using Microsoft.AspNetCore.Identity;

    public class JoinRequestsService : IJoinRequestsService
    {
        private readonly CheDbContext _dbContext;
        private readonly UserManager<CheUser> _userManager;
        private readonly ICooperativesService _cooperativesService;

        public JoinRequestsService(
            CheDbContext dbContext,
            UserManager<CheUser> userManager,
            ICooperativesService cooperativesService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._cooperativesService = cooperativesService;
        }

        public async Task<bool> CreateAsync(string content, string cooperativeId, string senderName)
        {
            var cooperative = await this._cooperativesService.GetByIdAsync<Cooperative>(cooperativeId);
            var receiver = await this._userManager.FindByNameAsync(cooperative.Creator.UserName);
            var sender = await this._userManager.FindByNameAsync(senderName);

            var request = new JoinRequest
            {
                Content = content,
                Sender = sender,
                Receiver = receiver,
                CooperativeId = cooperativeId,
                CreatedOn = DateTime.UtcNow
            };

            await this._dbContext.JoinRequests.AddAsync(request);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public Task<bool> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetCooperativeAllAsync<TEntity>(string cooperativeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetTeacherAllAsync<TEntity>(string teacherId)
        {
            throw new NotImplementedException();
        }
    }
}
