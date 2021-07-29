﻿namespace CHE.Web.Controllers
{
    using CHE.Common;
    using CHE.Data.Models;
    using CHE.Services.Data;
    using CHE.Web.Infrastructure;
    using CHE.Web.ViewModels;
    using CHE.Web.ViewModels.Teachers;

    using Microsoft.AspNetCore.Mvc;

    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeachersController : Controller
    {
        private const int DefaultPageSize = 6;

        private readonly ICheUsersService _cheUsersService;
        private readonly IReviewsService _reviewsService;
        private readonly IAddressesService _addressesService;

        public TeachersController(
            ICheUsersService cheUsersService,
            IReviewsService reviewsService,
            IAddressesService addressesService)
        {
            this._cheUsersService = cheUsersService;
            this._reviewsService = reviewsService;
            this._addressesService = addressesService;
        }

        public async Task<IActionResult> All(FilterViewModel filter, int pageIndex = 1)
        {
            var teachers = await this._cheUsersService.GetAllAsync<TeacherAllViewModel>(
                GlobalConstants.TeacherRole, 
                pageIndex, 
                DefaultPageSize, 
                filter.Level, 
                filter.City, 
                filter.Neighbourhood);

            var count = await this._cheUsersService.CountAsync(
                GlobalConstants.TeacherRole, 
                filter.Level, 
                filter.City, 
                filter.Neighbourhood);

            filter.LevelDisplayName = "school level";
            filter.Levels = Enum.GetValues(typeof(SchoolLevel))
                .Cast<SchoolLevel>()
                .Where(x => x.ToString() != "Unknown")
                .Select(x => x.ToString());
            filter.Cities = await this._addressesService.GetAllCitiesAsync();
            filter.Neighbourhoods = await this._addressesService.GetAllNeighbourhoodsAsync();

            var teachersList = new TeacherAllListViewModel
            {
                Teachers = PaginatedList<TeacherAllViewModel>
                 .Create(teachers, count, pageIndex, DefaultPageSize),
                Filter = filter
            };

            return View(teachersList);
        }

        public async Task<IActionResult> Details(string id)
        {
            var currentTeacher = await this._cheUsersService
                .GetByIdAsync<TeacherDetailsViewModel>(id);

            if (currentTeacher == null)
            {
                return this.NotFound();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                var userId = this.User.GetId();
                currentTeacher.SentReviewId = await this._reviewsService
                    .GetSentReviewIdAsync(userId, id);
            }
            
            this.ViewData["id"] = currentTeacher.Id;

            return this.View(currentTeacher);
        }
    }
}