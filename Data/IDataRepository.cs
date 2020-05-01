using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidData2020.Data.Model;

namespace CovidData2020.Data
{
    public interface IDataRepository
    {
        IEnumerable<Country> GetCountries();
        Country GetCountryById(int countryId);
        IEnumerable<DailyRecord> GetDailyRecordsByCountryId(int countryId);
        IEnumerable<DailyRecord> GetDailyRecordsByCountryIdAndPeriod(int countryId, int days);
        IEnumerable<DailyRecord> GetDailyRecords();
        void LoadExternalData();
        IEnumerable<DailyRecord> GetTotalRecordsNumber();
    }
}
