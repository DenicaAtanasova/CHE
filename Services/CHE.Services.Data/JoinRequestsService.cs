namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using CHE.Data;
    using CHE.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class JoinRequestsService : IJoinRequestsService
    {
        private readonly CheDbContext _dbContext;
        private readonly UserManager<CheUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ICooperativesService _cooperativesService;

        public JoinRequestsService(
            CheDbContext dbContext,
            UserManager<CheUser> userManager,
            IMapper mapper,
            ICooperativesService cooperativesService)
        {
            this._dbContext = dbContext;
            this._userManager = userManager;
            this._mapper = mapper;
            this._cooperativesService = cooperativesService;
        }

        public async Task<bool> CreateAsync(string content, string cooperativeId, string senderName)
        {
            var cooperative = await this._cooperativesService.GetByIdAsync<Cooperative>(cooperativeId);
            var receiver = await this._userManager.FindByNameAsync(cooperative.Creator.UserName);
            var sender = await this._userManager.FindByNameAsync(senderName);

            //TODO: check if request from the sender already exists
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
    }
}