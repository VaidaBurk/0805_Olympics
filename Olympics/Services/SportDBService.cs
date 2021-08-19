using Olympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Olympics.Services
{

    public class SportDBService
    {
        private readonly SqlConnection _connection;
        public SportDBService(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public List<SportModel> GetData()
        {
            List<SportModel> sports = new();
            _connection.Open();
            using var command = new SqlCommand("SELECT * FROM sports ORDER BY 2", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                sports.Add(new()
                {
                    SportId = reader.GetInt32(0),
                    SportType = reader.GetString(1),
                    TeamActivity = reader.GetBoolean(2)
                });
            }
            _connection.Close();
            return (sports);
        }

        public void SaveToDatabase(SportModel sport)
        {
            _connection.Open();
            SqlCommand command = new($"INSERT INTO sports (sport_type, team_activity) VALUES ('{sport.SportType}', '{sport.TeamActivity}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
