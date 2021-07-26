namespace CHE.Services.Data.Models
{
    using System;

    [Flags]
    public enum CooperativeUser
    {
        Admin = 0,
        Member = 1,
        Other = 2
    }
}