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

        public JoinRequestsService(
            CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
            => await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllByTeacherAsync<TEntity>(string teacherId)
            => await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.ReceiverId == teacherId)
                .To<TEntity>()
                .ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllByCooperativeAsync<TEntity>(string cooperativeId)
            => await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.CooperativeId == cooperativeId)
                .To<TEntity>()
                .ToListAsync();

        public async Task<string> CreateAsync(string content, string cooperativeId, string senderId, string receiverId)
        {
            var requestId = await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.CooperativeId == cooperativeId &&
                            x.ReceiverId == receiverId &&
                            x.SenderId == senderId)
                .Select(x=> x.Id)
                .SingleOrDefaultAsync();

            if (requestId is null)
            {
                var request = new JoinRequest
                {
                    Content = content,
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    CooperativeId = cooperativeId,
                    CreatedOn = DateTime.UtcNow
                };

                this._dbContext.JoinRequests.Add(request);
                await this._dbContext.SaveChangesAsync();

                return request.Id;
            }          

            return requestId;
        }
    }
}