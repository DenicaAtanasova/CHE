﻿namespace CHE.Web.Controllers
{
    using CHE.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    using System.Diagnostics;

    public class HomeController : Controller
    {
        public IActionResult Index() => 
            this.View();

        public IActionResult Privacy() =>
            this.View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}