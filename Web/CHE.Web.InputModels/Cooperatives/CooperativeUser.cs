namespace CHE.Web.InputModels.Cooperatives
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