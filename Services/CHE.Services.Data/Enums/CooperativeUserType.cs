namespace CHE.Services.Data.Enums
{
    using System;

    [Flags]
    public enum CooperativeUserType
    {
        Admin = 0,
        Member = 1,
        Other = 2
    }
}