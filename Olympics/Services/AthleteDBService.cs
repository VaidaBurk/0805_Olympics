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

        //private List<AthleteModel> FetchedAthletes()
        //{
        //    List<AthleteModel> athletes = new();
        //    _connection.Open();
        //    using var command = new SqlCommand("SELECT * FROM athletes_with_country_name", _connection);
        //    using var reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        athletes.Add(new()
        //        {
        //            Id = reader.GetInt32(0),
        //            Name = reader.GetString(1),
        //            Surname = reader.GetString(2),
        //            CountryId = reader.GetInt32(3),
        //            CountryName = reader.GetString(4)
        //        });
        //    }
        //    _connection.Close();
        //    return (athletes);
        //}

        public List<AthleteModel> GetData()
        {
            List<AthleteModel> athletes = new(); 
            _connection.Open();
            using var command = new SqlCommand($@"SELECT a.id, a.name, a.surname, a.country_id, c.country_name, STRING_AGG(s.sport_type, ', ') AS 'sports'
                                                FROM athletes a
                                                JOIN countries c
                                                ON a.country_id = c.country_id
                                                JOIN athletes_sports ats
                                                ON a.id = ats.athlete_id
                                                JOIN sports s
                                                ON ats.sport_id = s.sport_id
                                                GROUP BY a.id, a.name, a.surname, a.country_id, c.country_name", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athletes.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Surname = reader.GetString(2),
                    CountryId = reader.GetInt32(3),
                    CountryName = reader.GetString(4),
                    Sports = reader.GetString(5)
                });
            }
            _connection.Close();
            return athletes;
        }

        public void SaveToDatabase(ParticipantModel participant)
        {
            _connection.Open();
            SqlCommand command = new($@"INSERT INTO athletes (name, surname, country_id)
                    VALUES ('{participant.Athletes[0].Name}', '{participant.Athletes[0].Surname}', '{participant.Athletes[0].CountryId}');", _connection);
            command.ExecuteNonQuery();
            _connection.Close();

            int id = GetAthleteId(participant);

          
            foreach (int entry in participant.Sports)
            {
                _connection.Open();

                command = new($@"INSERT INTO athletes_sports (athlete_id, sport_id) 
                    VALUES ('{id}', '{entry}');", _connection);
                    
            command.ExecuteNonQuery();
            _connection.Close();
            }
        }

        public int GetAthleteId(ParticipantModel participant)
        {
            _connection.Open();
            SqlCommand command = new($@"SELECT MAX(id) FROM athletes 
                                        WHERE name = '{participant.Athletes[0].Name}' 
                                        AND surname = '{participant.Athletes[0].Surname}'", _connection);
            int id = (Int32)command.ExecuteScalar();
            _connection.Close();
            return id;
        }

        //public List<AthleteModel> FilterByCountry(string countryName)
        //{
        //    List<AthleteModel> athletes = new();
        //    _connection.Open();
        //    using var command = new SqlCommand($@"SELECT a.id, a.name, a.surname, a.country_id, c.country_name, STRING_AGG(s.sport_type, ', ') AS 'sports'
        //                                        FROM athletes a
        //                                        JOIN countries c
        //                                        ON a.country_id = c.country_id
        //                                        JOIN athletes_sports ats
        //                                        ON a.id = ats.athlete_id
        //                                        JOIN sports s
        //                                        ON ats.sport_id = s.sport_id
        //                                        GROUP BY a.id, a.name, a.surname, a.country_id, c.country_name
        //                                        HAVING c.country_name = {countryName}", _connection);
        //    using var reader = command.ExecuteReader();
        //    while (reader.Read())
        //    {
        //        athletes.Add(new()
        //        {
        //            Id = reader.GetInt32(0),
        //            Name = reader.GetString(1),
        //            Surname = reader.GetString(2),
        //            CountryId = reader.GetInt32(3),
        //            CountryName = reader.GetString(4),
        //            Sports = reader.GetString(5)
        //        });
        //    }
        //    _connection.Close();
        //    return athletes;
        //}
    }
}
