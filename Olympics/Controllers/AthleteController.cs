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
    public class AthleteController : Controller
    {
        private readonly SqlConnection _connection;
        private AthleteDBService _athleteDBService;
        public AthleteController(SqlConnection connection, AthleteDBService athleteDBService)
        {
            _connection = connection;
            _athleteDBService = athleteDBService;
        }

        public IActionResult List()
        {
            List<AthleteModel> athletes = _athleteDBService.GetData(_connection);
            return View(athletes);
        }

        public IActionResult DisplayCreate()
        {
            return View("Create");
        }

        public IActionResult Create(AthleteModel athlete)
        {
            _athleteDBService.SaveToDatabase(athlete, _connection);
            return RedirectToAction("List");
        }


    }
}
