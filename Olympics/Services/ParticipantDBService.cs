using Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Services
{
    public class ParticipantDBService
    {
        public AthleteDBService AthleteDBService;
        public CountryDBService CountryDBService;
        public SportDBService SportDBService;

        public ParticipantDBService(AthleteDBService athleteDBService, CountryDBService countryDBService, SportDBService sportDBService)
        {
            AthleteDBService = athleteDBService;
            CountryDBService = countryDBService;
            SportDBService = sportDBService;

        }

        public ParticipantModel GetData()
        {
            return new ParticipantModel()
            {
                Athletes = AthleteDBService.GetData(),
                Countries = CountryDBService.GetData(),
                SportModels = SportDBService.GetData()
            };
        }
        public ParticipantModel newAthlete()
        {
            List <AthleteModel > athletes = new List<AthleteModel>();
            athletes.Add(new AthleteModel());

            return new ParticipantModel()
            {
                Athletes = athletes,
                Countries = CountryDBService.GetData(),
                SportModels = SportDBService.GetData(),
                Sports = new List<int>()
            };
        }
    }
}
