namespace CHE.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Email => "Email";

        public static string ChangePassword => "ChangePassword";

        public static string PersonalData => "PersonalData";

        public static string Portfolio => "Portfolio";

        public static string JoinRequest => "JoinRequest";

        public static string Review => "Review";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string EmailNavClass(ViewContext viewContext) => PageNavClass(viewContext, Email);

        public static string ChangePasswordNavClass(ViewContext viewContext) => PageNavClass(viewContext, ChangePassword);

        public static string PersonalDataNavClass(ViewContext viewContext) => PageNavClass(viewContext, PersonalData);

        public static string PortfolioNavClass(ViewContext viewContext) => PageNavClass(viewContext, Portfolio);

        public static string JoinRequestNavClass(ViewContext viewContext) => PageNavClass(viewContext, JoinRequest);

        public static string ReviewNavClass(ViewContext viewContext) => PageNavClass(viewContext, Review);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}