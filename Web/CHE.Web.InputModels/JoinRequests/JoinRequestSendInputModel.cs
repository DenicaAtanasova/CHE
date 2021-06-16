﻿namespace CHE.Web.InputModels.JoinRequests
{
    using System.ComponentModel.DataAnnotations;

    public class JoinRequestCreateInputModel
    {
        [Required]
        public string Content { get; set; }

        public string ReceiverId { get; set; }

        [Required]
        public string CooperativeId { get; set; }
    }
}