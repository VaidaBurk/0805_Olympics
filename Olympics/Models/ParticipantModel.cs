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
    }
}
