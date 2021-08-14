using Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Services
{

    public class CountryDBService
    {
        private readonly SqlConnection _connection;
        public CountryDBService(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public List<CountryModel> GetData()
        {
            List<CountryModel> countries = new();
            _connection.Open();
            using var command = new SqlCommand("SELECT * FROM countries ORDER BY country_name", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                countries.Add(new()
                {
                    CountryId = reader.GetInt32(0),
                    CountryName = reader.GetString(1),
                    CountryCode = reader.GetString(2)
                });
            }
            _connection.Close();
            return (countries);
        }

        public void SaveToDatabase(CountryModel country)
        {
            _connection.Open();
            SqlCommand command = new($"INSERT INTO countries (country_name, country_code) VALUES ('{country.CountryName}', '{country.CountryCode}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
