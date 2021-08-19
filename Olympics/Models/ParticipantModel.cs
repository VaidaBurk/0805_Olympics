using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Models
{
    public class ParticipantModel
    {
        public List<CountryModel> Countries { get; set; }
        public List<AthleteModel> Athletes { get; set; }
        public List<SportModel> SportModels { get; set; }
        public List<int> Sports { get; set; }
        public int FilterByCountryId { get; set; }
        public int FilterBySportId { get; set; }
        public int FilterIsTeamSport { get; set; }
        public int SortBy { get; set; }


        public ParticipantModel(List<AthleteModel> athletes, List<CountryModel> countries, List<SportModel> sportModels)
        {
            Athletes = athletes;
            Countries = countries;
            SportModels = sportModels;
        }

        public ParticipantModel()
        {

        }
    }
}
