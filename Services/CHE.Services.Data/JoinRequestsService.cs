namespace CHE.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;

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

        public async Task<bool> CreateAsync(string content, string cooperativeId, string senderId, string receiverId)
        { 
            var memberExists = this._dbContext.Cooperatives
                .Where(x => x.Id == cooperativeId)
                .Any(x => x.Members.Any(m => m.CheUserId == senderId));

            if (memberExists)
            {
                return false;
            }
            
            var request = new JoinRequest
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,
                CooperativeId = cooperativeId,
                CreatedOn = DateTime.UtcNow
            };

            await this._dbContext.JoinRequests.AddAsync(request);
            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var requestFromDb = await this._dbContext
                .JoinRequests
                .Where(x => x.Id == id)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return requestFromDb;
        }

        public async Task<bool> SendAsync(string requestContent, string cooperativeId, string receiverId, string senderName)
        {
            var sender = await this._userManager.FindByNameAsync(senderName);

            var result = await this.CreateAsync(requestContent, cooperativeId, sender.Id, receiverId);

            return result;
        }

        public async Task<bool> AcceptAsync(string requestId)
        {
            var request = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == requestId);

            await this._cooperativesService
                .AddMemberAsync(request.SenderId, request.CooperativeId);

            this.Delete(request);

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool> RejectAsync(string requestId)
        {
            var requestToDelete = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == requestId);

            this.Delete(requestToDelete);

            var result = await this._dbContext.SaveChangesAsync() > 0;

            return result;
        }

        private bool Delete(JoinRequest request)
        {
            request.IsDeleted = true;
            request.DeletedOn = DateTime.UtcNow;

            var result = this._dbContext.Update(request).State == EntityState.Modified;

            return result;
        }
    }
}