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
    public class CountryController : Controller
    {
        private CountryDBService _countryDBService;
        public CountryController(CountryDBService countryDBService)
        {
            _countryDBService = countryDBService;
        }

        public IActionResult List()
        {
            List<CountryModel> countries = _countryDBService.GetData();
            return View(countries);
        }

        public IActionResult DisplayCreate()
        {
            CountryModel country = new();
            return View("Create", country);
        }

        public IActionResult Create(CountryModel country)
        {
            _countryDBService.SaveToDatabase(country);
            return RedirectToAction("List");
        }
    }
}
