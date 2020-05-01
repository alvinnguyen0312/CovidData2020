using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CovidData2020.Data.Model;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace CovidData2020.Data
{
    public class DataRepository :IDataRepository
    {
        private readonly string _connectionString;

        public DataRepository(IConfiguration configuration) //pass configuration into constructor using Dependency injection
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        public IEnumerable<Country> GetCountries()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<Country>(@"SELECT * FROM dbo.Country");
            }
        }

        public Country GetCountryById(int countryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirst<Country>(@"SELECT * FROM dbo.Country WHERE CountryId = @CountryId", new { CountryId = countryId });
            }
        }

        public IEnumerable<DailyRecord> GetDailyRecordsByCountryId(int countryId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<DailyRecord>(
                    @" SELECT TOP (1) *
                FROM dbo.CovidDailyRecord dr INNER JOIN dbo.Country c
                ON c.CountryId = dr.CountryId
                WHERE c.CountryId = @CountryId ORDER BY dr.RecordId DESC", new { CountryId = countryId });
            }
        }
        public IEnumerable<DailyRecord> GetDailyRecordsByCountryIdAndPeriod(int countryId, int days)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<DailyRecord>(
                    @" SELECT TOP (@Days) *
                FROM dbo.CovidDailyRecord dr INNER JOIN dbo.Country c
                ON c.CountryId = dr.CountryId
                WHERE c.CountryId = @CountryId ORDER BY dr.RecordId DESC", new { Days = days, CountryId = countryId });
            }
        }
        public IEnumerable<DailyRecord> GetTotalRecordsNumber()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<DailyRecord>(
                   @"SELECT TOP (1) SUM(Confirmed) AS Confirmed, SUM(Deaths) AS Deaths, SUM(Recovered) AS Recovered, 
                    RecordDate FROM dbo.CovidDailyRecord GROUP BY RecordDate ORDER BY RecordDate DESC");
            }
        }

        public IEnumerable<DailyRecord> GetDailyRecords()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<DailyRecord>(@"SELECT * FROM dbo.CovidDailyRecord");
            }
        }

        //Load json data into tables
        public void LoadExternalData()
        {
            try
            {
                WebClient client = new WebClient();
                string URL = client.DownloadString("https://pomber.github.io/covid19/timeseries.json");
                var objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, DailyRecord[]>>(URL);
                LoadCountryTable(objectJson);
                LoadDailyRecordsTable(objectJson);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //Load country table
        public void LoadCountryTable(Dictionary<string, DailyRecord[]> objectJson)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //delete records before adding new ones
                connection.Execute(@"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Country') DELETE FROM dbo.Country");
                try
                {
                    foreach (var c in objectJson)
                    {
                        connection.Execute(@"EXEC dbo.Country_Post @CountryName = @CountryName", new { CountryName = c.Key });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //Load Daily Record Table
        public void LoadDailyRecordsTable(Dictionary<string, DailyRecord[]> objectJson)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //delete records before adding new ones
                connection.Execute(@"IF EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CovidDailyRecord') DELETE FROM dbo.CovidDailyRecord");
                try
                {
                    foreach (var country in objectJson)
                    {
                        foreach (var rec in country.Value)
                        {
                            connection.Execute(
                            @"EXEC dbo.DailyRecord_Post
                            @CountryName = @CountryName, @Confirmed = @Confirmed,
                            @Deaths = @Deaths, @Recovered = @Recovered,
                            @RecordDate = @RecordDate", new { CountryName = country.Key, rec.Confirmed, rec.Deaths, rec.Recovered, RecordDate = rec.RecordDate });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }
}
