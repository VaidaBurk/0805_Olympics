using Microsoft.AspNetCore.Mvc;
using Olympics.Models;
using Olympics.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Controllers
{
    public class SportController : Controller
    {
        private SportDBService _sportDBService;
        public SportController(SportDBService sportDBService)
        {
            _sportDBService = sportDBService;
        }

        public IActionResult List()
        {
            List<SportModel> sports = _sportDBService.GetData();
            return View(sports);
        }

        public IActionResult DisplayCreate()
        {
            SportModel sport = new();
            return View("Create", sport);
        }

        public IActionResult Create(SportModel sport)
        {
            _sportDBService.SaveToDatabase(sport);
            return RedirectToAction("List");
        }
    }
}
