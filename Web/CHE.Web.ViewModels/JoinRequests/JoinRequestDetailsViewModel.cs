﻿namespace CHE.Web.ViewModels.JoinRequests
{
    using CHE.Data.Models;
    using CHE.Services.Mapping;

    public class JoinRequestDetailsViewModel : IMapFrom<JoinRequest>
    {
        public string Id { get; init; }

        public string Content { get; init; }

        public string SenderId { get; init; }

        public string SenderUserUserName { get; init; }

        public string CooperativeId { get; init; }

        public string CooperativeName { get; init; }
    }
}