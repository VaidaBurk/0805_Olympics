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
        private ParticipantDBService _dbService;

        public AthleteController(ParticipantDBService dbService)
        {
            _dbService = dbService;
        }

        public IActionResult List()
        {
            //List<AthleteModel> athletes = _dbService.AthleteDBService.GetData();
            //ParticipantModel participants = _dbService.AthleteDBService.GetAthleteData();
            return View(_dbService.AthleteDBService.GetData());
        }

        public IActionResult DisplayCreate()
        {
            ParticipantModel participant = _dbService.newAthlete();          
            return View("Create", participant);
        }

        public IActionResult Create(ParticipantModel participant)
        {
            _dbService.AthleteDBService.SaveToDatabase(participant);
            return RedirectToAction("List");
        }

        //public IActionResult FilterAthletesByCountry()
        //{
        //    return View();
        //}
    }
}
