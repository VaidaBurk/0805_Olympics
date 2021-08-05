using Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Services
{
    public class AthleteDBService
    {
        
        public List<AthleteModel> GetData(SqlConnection connection)
        {
            List<AthleteModel> athletes = new();
            connection.Open();
            using var command = new SqlCommand("SELECT * FROM Athletes", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athletes.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    Country = reader.GetString(3)
                });
            }
            connection.Close();
            return (athletes);
        }

        public void SaveToDatabase(AthleteModel athlete, SqlConnection connection)
        {
            connection.Open();
            SqlCommand command = new($"INSERT INTO Athletes (Name, Surname, Country) VALUES ('{athlete.Name}', '{athlete.Surname}', '{athlete.Country}');", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

    }
}
