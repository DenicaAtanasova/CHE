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
        private readonly IMapper _mapper;

        public JoinRequestsService(
            CheDbContext dbContext,
            IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
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

        public async Task<bool> DeleteAsync(string id)
        {
            var requestToDelete = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == id);

            requestToDelete.IsDeleted = true;
            requestToDelete.DeletedOn = DateTime.UtcNow;

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

        public async Task<ICollection<TEntity>> GetAllUnDeletedByCooperativeId<TEntity>(string cooperativeId)
        {
            var cooperativeRequests = await this._dbContext.JoinRequests
                .Where(x => x.CooperativeId == cooperativeId && x.IsDeleted == false)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .ToArrayAsync();

            return cooperativeRequests;
        }
    }
}