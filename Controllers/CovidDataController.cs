using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CovidData2020.Data.Model;
using CovidData2020.Data;


namespace CovidData2020.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidDataController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;

        public CovidDataController(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        [HttpGet("country")]
        public IEnumerable<Country> GetCountries()
        {
            //_dataRepository.LoadExternalData();
            return _dataRepository.GetCountries();
        }
        [HttpGet("total")]
        public IEnumerable<DailyRecord> GetTotalRecordsNumber()
        {
            //_dataRepository.LoadExternalData();
            return _dataRepository.GetTotalRecordsNumber();
        }
        // GET: api/<controller>
        [HttpGet()]
        public IEnumerable<DailyRecord> GetDailyRecords()
        {
            //_dataRepository.LoadExternalData();
            return _dataRepository.GetDailyRecords();
        }

        [HttpGet("country/{countryId}")]
        public ActionResult<IEnumerable<DailyRecord>> GetDailyRecordsByCountryId(int countryId, int period)
        {

            var records = period > 0 ? _dataRepository.GetDailyRecordsByCountryIdAndPeriod(countryId, period) :
                _dataRepository.GetDailyRecordsByCountryId(countryId);
            if (records.ToList().Count == 0)
            {
                return NotFound();
            }
            return records.ToList();
        }

    }
}
