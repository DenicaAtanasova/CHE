namespace CHE.Services.Data.Tests
{
    using CHE.Data;
    using CHE.Data.Models;
    using CHE.Services.Data.Tests.Mocks;
    using CHE.Services.Mapping;
    using CHE.Web.ViewModels.Messages;

    using Microsoft.EntityFrameworkCore;

    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class MessagesServiceTests
    {
        private readonly CheDbContext _dbContext;
        private readonly IMessagesService _messagesService;

        public MessagesServiceTests()
        {
            this._dbContext = DatabaseMock.Instance;
            this._messagesService = new MessagesService(this._dbContext);

            AutoMapperConfig.RegisterMappings(typeof(MessageViewModel).Assembly);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectMessage()
        {
            var message = new Message
            {
                Text = "Text"
            };

            this._dbContext.Messages.Add(message);
            await this._dbContext.SaveChangesAsync();

            var messageFromDb = await this._messagesService
                .GetByIdAsync<MessageViewModel>(message.Id);

            Assert.Equal(message.Text, messageFromDb.Text);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewMessage()
        {
            var messengerId = Guid.NewGuid().ToString();
            var sender = "Sender name";
            var text = "some text";

            var messageId = await this._messagesService
                .CreateAsync(messengerId, sender, text);

            var messageFromDb = await this._dbContext.Messages
                .SingleOrDefaultAsync();
            var ecpectedCreatedOn = DateTime.UtcNow;

            Assert.Equal(messageId, messageFromDb.Id);
            Assert.Equal(messengerId, messageFromDb.MessengerId);
            Assert.Equal(sender, messageFromDb.Sender);
            Assert.Equal(text, messageFromDb.Text);
            Assert.Equal(ecpectedCreatedOn,
                messageFromDb.CreatedOn,
                new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: 1000));
        }
    }
}
