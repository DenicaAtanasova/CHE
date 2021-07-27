namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.ParentRole)]
    [Authorize(Roles = GlobalConstants.ParentRole)]
    public abstract class ParentController : Controller
    {
    }
}