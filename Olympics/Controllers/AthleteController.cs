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
            List<AthleteModel> athletes = _dbService.AthleteDBService.GetData();
            return View(athletes);
        }

        public IActionResult DisplayCreate()
        {
            ParticipantModel participant = new()
            {
                Athletes = new List<AthleteModel>()
                {
                    new AthleteModel()
                    {

                    }
                },

                Countries = _dbService.CountryDBService.GetData(),
                SportModels = _dbService.SportDBService.GetData()
            };           
            return View("Create", participant);
        }

        public IActionResult Create(ParticipantModel participant)
        {
            _dbService.AthleteDBService.SaveToDatabase(participant);
            return RedirectToAction("List");
        }
    }
}
