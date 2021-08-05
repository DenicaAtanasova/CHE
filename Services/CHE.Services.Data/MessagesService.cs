namespace CHE.Services.Data
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MessagesService : IMessagesService
    {
        private readonly CheDbContext _dbContext;

        public MessagesService(CheDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<string> CreateAsync(string messengerId, string senderName, string message)
        {
            var currentMessage = new Message
            {
                Sender = senderName,
                Text = message,
                CreatedOn = DateTime.UtcNow,
                MessengerId = messengerId
            };

            this._dbContext.Messages.Add(currentMessage);
            await this._dbContext.SaveChangesAsync();

            return currentMessage.Id;
        }

        public async Task<TEntity> GetById<TEntity>(string id) =>
            await this._dbContext.Messages
            .Where(x => x.Id == id)
            .To<TEntity>()
            .SingleOrDefaultAsync();
    }
}