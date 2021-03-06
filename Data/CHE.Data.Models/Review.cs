﻿namespace CHE.Data.Models
{
    using Common.Models;

    public class Review : BaseModel<string>
    {
        public string Comment { get; set; }

        public int Rating { get; set; }

        public string SenderId { get; set; }

        public CheUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public CheUser Receiver { get; set; }
    }
}