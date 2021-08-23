namespace CHE.Web
{
    public record WebConstants
    {
        public const string AccountLayout = "/Areas/Identity/Pages/Account/Manage/_Layout.cshtml";
        public const string TeacherLayout = "/Views/Shared/_LayoutTeacher.cshtml";
        public const string CooperativeLayout = "/Views/Shared/_LayoutCooperative.cshtml";

        public const int DefaultPageSize = 8;
    }
}