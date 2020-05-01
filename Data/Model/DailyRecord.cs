using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidData2020.Data.Model
{
    public class DailyRecord
    {
       public int RecordId { get; set; }
        public int CountryId { get; set; }
        public int Confirmed { get; set; }
        public int Deaths { get; set; }
        public int Recovered { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
