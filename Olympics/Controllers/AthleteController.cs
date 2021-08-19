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
            return View(_dbService.GetData());
        }

        public IActionResult ListFiltered(ParticipantModel model)
        {
           ParticipantModel dbModel = _dbService.GetData();

            if (model.FilterByCountryId != 0)
            {
                //List<AthleteModel> filteredAthletes = dbModel.Athletes.Where(a => a.CountryId == model.FilterByCountryId).ToList();
                //dbModel.Athletes = filteredAthletes;
                dbModel = _dbService.GetFilteredByCountryData(model.FilterByCountryId);
                return View("List", dbModel);
            }
            if(model.FilterBySportId != 0)
            {
                dbModel = _dbService.GetFilteredData(model.FilterBySportId);
                return View("List", dbModel);
            }
            if (model.FilterIsTeamSport != 0)
            {
                dbModel = _dbService.GetTeamSportData(model.FilterIsTeamSport);
                return View("List", dbModel);
            }
            if(model.SortBy != 0)
            {
                dbModel = _dbService.GetSortedData(model.SortBy);
            }
            return View("List", dbModel);
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

        public IActionResult FilterByCountry()
        {
            return View("List");
        }
    }
}
