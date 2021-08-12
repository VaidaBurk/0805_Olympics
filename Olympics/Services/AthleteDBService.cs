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
        private readonly SqlConnection _connection;
        public AthleteDBService(SqlConnection connection)
        {
            _connection = connection;
        }
        
        public List<AthleteModel> GetData()
        {
            List<AthleteModel> athletes = new();
            _connection.Open();
            using var command = new SqlCommand("SELECT * FROM athletes_with_country_name", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athletes.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    CountryId = reader.GetInt32(3),
                    CountryName = reader.GetString(4)
                });
            }
            _connection.Close();
            return (athletes);
        }

        public void SaveToDatabase(ParticipantModel participant)
        {
            _connection.Open();
            SqlCommand command = new($@"INSERT INTO athletes (name, surname, country_id)
                    VALUES ('{participant.Athletes[0].Name}', '{participant.Athletes[0].Surname}', '{participant.Athletes[0].CountryId}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();

            int id = GetAthleteId(participant);

            _connection.Open();
            command = new($@"INSERT INTO athletes_sports (athlete_id, sport_id) 
                    VALUES ('{id}', '{participant.SportModels[0].SportId}');", _connection);
                    
            command.ExecuteNonQuery();
            _connection.Close();
        }

        public int GetAthleteId(ParticipantModel participant)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand($@"
                SELECT MAX(id) FROM athletes 
                WHERE name = '{participant.Athletes[0].Name}' AND surname = '{participant.Athletes[0].Surname}'", _connection);
            int id = (Int32)command.ExecuteScalar();
            _connection.Close();
            return id;
        }
    }
}
