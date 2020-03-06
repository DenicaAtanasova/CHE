namespace CHE.Web.Areas.Parent.Controllers
{
    using CHE.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(GlobalConstants.PARENT_ROLE)]
    [Authorize(Roles = GlobalConstants.PARENT_ROLE)]
    public abstract class ParentController : Controller
    {
    }
}