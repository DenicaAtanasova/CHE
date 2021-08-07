namespace CHE.Web.Infrastructure
{
    using CHE.Common;
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string GetId(this ClaimsPrincipal user) => 
            user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public static bool IsTeacher(this ClaimsPrincipal user)
            => user.IsInRole(GlobalConstants.TeacherRole);

        public static bool IsParent(this ClaimsPrincipal user)
            => user.IsInRole(GlobalConstants.ParentRole);
    }
}