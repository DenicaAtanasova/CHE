namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class JoinRequestsService : IJoinRequestsService
    {
        private readonly CheDbContext _dbContext;

        public JoinRequestsService(
            CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id) =>
            await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.Id == id)
                .To<TEntity>()
                .SingleOrDefaultAsync();

        public async Task<string> GetPendindRequestIdAsync(string cooperativeId, string senderId) =>
            await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.SenderId == senderId && x.CooperativeId == cooperativeId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllByTeacherAsync<TEntity>(string teacherId) =>
            await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.ReceiverId == teacherId)
                .To<TEntity>()
                .ToListAsync();

        public async Task<IEnumerable<TEntity>> GetAllByCooperativeAsync<TEntity>(string cooperativeId) =>
            await this._dbContext.JoinRequests
                .AsNoTracking()
                .Where(x => x.CooperativeId == cooperativeId && x.ReceiverId == null)
                .To<TEntity>()
                .ToListAsync();

        public async Task<string> CreateAsync(
            string senderId,
            string content,
            string cooperativeId,
            string receiverId)
        {
            var request = new JoinRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                CooperativeId = cooperativeId,
                CreatedOn = DateTime.UtcNow
            };

            this._dbContext.JoinRequests.Add(request);
            await this._dbContext.SaveChangesAsync();

            return request.Id;
        }

        public async Task UpdateAsync(string id, string content)
        {
            var joinRequestToUpdate = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == id);

            if (joinRequestToUpdate == null)
            {
                return;
            }

            joinRequestToUpdate.Content = content;
            joinRequestToUpdate.ModifiedOn = DateTime.UtcNow;

            this._dbContext.JoinRequests.Update(joinRequestToUpdate);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var request = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return;
            }

            this._dbContext.JoinRequests.Remove(request);
            await this._dbContext.SaveChangesAsync();
        }
    }
}