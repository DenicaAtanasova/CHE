namespace CHE.Web.Hubs
{
    using CHE.Services.Data;
    using CHE.Web.ViewModels.Messages;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Authorize]
    public class MessengerHub : Hub
    {
        private static readonly IDictionary<string, string> usersWithStatus =
            new ConcurrentDictionary<string, string>();

        private readonly IMessagesService _messagesService;

        public MessengerHub(IMessagesService messagesService)
        {
            this._messagesService = messagesService;
        }

        public async Task SendPrivate(string messengerId, string receiverId, string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var senderName = this.Context.User.Identity.Name;
            var messageId = await this._messagesService
                .CreateAsync(messengerId, senderName, text);

            var message = await this._messagesService
                .GetById<MessageViewModel>(messageId);

            await this.Clients.User(receiverId)
                .SendAsync("NewMessage", message, receiverId);

            await this.Clients.Caller
                .SendAsync("newMessage", message, senderName);
        }

        [Authorize("CooperativeMembersRestricted")]
        public async Task SendGroup(string messengerId, string cooperativeId, string text)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            await Groups.AddToGroupAsync(this.Context.ConnectionId, cooperativeId);

            var senderName = this.Context.User.Identity.Name;
            var messageId = await this._messagesService
                .CreateAsync(messengerId, senderName, text);
            
            var message = await this._messagesService
                .GetById<MessageViewModel>(messageId);

            var connectionId = this.Context.ConnectionId;

            await this.Clients.Group(cooperativeId)
                .SendAsync("NewMessage", message, senderName, connectionId);
        }

        public override async Task OnConnectedAsync()
        {
            usersWithStatus[this.Context.UserIdentifier] = "online";

            await this.Clients.All.SendAsync(
                "UpdateStatus", usersWithStatus);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            usersWithStatus[this.Context.UserIdentifier] = "offline";

            await this.Clients.All.SendAsync(
                "UpdateStatus", usersWithStatus);

            await this.Clients.All.SendAsync("Disconnect");

            await base.OnDisconnectedAsync(exception);
        }
    }
}