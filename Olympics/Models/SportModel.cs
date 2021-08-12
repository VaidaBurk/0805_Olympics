using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Models
{
    public class SportModel
    {
        public int SportId { get; set; }
        public string SportType { get; set; }
        public bool TeamActivity { get; set; }
    }
}
