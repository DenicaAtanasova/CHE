namespace CHE.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CHE.Data;
    using CHE.Data.Models;

    public class JoinRequestsService : IJoinRequestsService
    {
        private readonly CheDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICooperativesService _cooperativesService;

        public JoinRequestsService(
            CheDbContext dbContext,
            IMapper mapper,
            ICooperativesService cooperativesService)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._cooperativesService = cooperativesService;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(string id)
        {
            var requestFromDb = await this._dbContext.JoinRequests
                .Where(x => x.Id == id)
                .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return requestFromDb;
        }

        public async Task<IEnumerable<TEntity>> GetTeacherAllAsync<TEntity>(string teacherId)
        {
            var requests = await this._dbContext.JoinRequests
                 .Where(x => x.ReceiverId == teacherId && !x.IsDeleted)
                 .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                 .ToArrayAsync();

            return requests;
        }

        public async Task<IEnumerable<TEntity>> GetCooperativeAllAsync<TEntity>(string cooperativeId)
        {
            var requests = await this._dbContext.JoinRequests
                 .Where(x => x.CooperativeId == cooperativeId && !x.IsDeleted)
                 .ProjectTo<TEntity>(this._mapper.ConfigurationProvider)
                 .ToArrayAsync();

            return requests;
        }

        public async Task<bool> CreateAsync(string content, string cooperativeId, string receiverId, string senderId)
        {
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

        private bool Delete(JoinRequest request)
        {
            request.IsDeleted = true;
            request.DeletedOn = DateTime.UtcNow;

            var result = this._dbContext.Update(request).State == EntityState.Modified;

            return result;
        }
    }
}