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
    }
}
