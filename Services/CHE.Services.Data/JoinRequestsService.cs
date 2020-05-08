namespace CHE.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestsService : IJoinRequestsService
    {
        private readonly CheDbContext _dbContext;
        private readonly ICooperativesService _cooperativesService;

        public JoinRequestsService(
            CheDbContext dbContext,
            ICooperativesService cooperativesService)
        {
            this._dbContext = dbContext;
            this._cooperativesService = cooperativesService;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var requestFromDb = await this._dbContext.JoinRequests
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

            return requestFromDb;
        }

        public async Task<IEnumerable<TEntity>> GetTeacherAllAsync<TEntity>(string teacherId)
        {
            var requests = await this._dbContext.JoinRequests
                 .Where(x => x.ReceiverId == teacherId)
                 .To<TEntity>()
                 .ToArrayAsync();

            return requests;
        }

        public async Task<bool> CreateAsync(string content, string cooperativeId, string receiverId, string senderId)
        {
            // TODO: check if such a request already exists
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

        public async Task<bool> AcceptAsync(string requestId)
        {
            var request = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == requestId);

            // request is send from parent to cooperative
            if (request.ReceiverId == null)
            {
                await this._cooperativesService
                    .AddMemberAsync(request.SenderId, request.CooperativeId);
            }
            // request is send from cooperative to teacher 
            else
            {
                await this._cooperativesService
                    .AddMemberAsync(request.ReceiverId, request.CooperativeId);
            }

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

        private void Delete(JoinRequest request)
        {
            this._dbContext.Remove(request);
        }
    }
}