﻿namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;
    using CHE.Web.InputModels.JoinRequests;

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

        public async Task<string> CreateAsync(string senderId, JoinRequestCreateInputModel inputModel)
        {
            var request = inputModel.Map<JoinRequestCreateInputModel, JoinRequest>();
            request.CreatedOn = DateTime.UtcNow;
            request.SenderId = senderId;

            this._dbContext.JoinRequests.Add(request);
            await this._dbContext.SaveChangesAsync();

            return request.Id;
        }

        public async Task UpdateAsync(JoinRequestUpdateInputModel inputModel)
        {
            var joinRequestToUpdate = await this._dbContext.JoinRequests
                .SingleOrDefaultAsync(x => x.Id == inputModel.Id);

            if (joinRequestToUpdate == null)
            {
                return;
            }

            joinRequestToUpdate.Content = inputModel.Content;
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