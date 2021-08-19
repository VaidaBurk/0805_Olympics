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
                                                GROUP BY a.id, a.name, a.surname, a.country_id, c.country_name
                                                ", _connection);
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

        public List<AthleteModel> GetFilteredBySportData(int sportId)
        {
            List<AthleteModel> athletes = new();
            _connection.Open();
            using var command = new SqlCommand($@"
                                                SELECT a.name, a.surname, c.country_name, s.sport_type
                                                FROM athletes a
                                                JOIN athletes_sports ats
                                                ON a.id = ats.athlete_id
                                                JOIN countries c
                                                ON a.country_id = c.country_id
                                                JOIN sports s
                                                ON ats.sport_id = s.sport_id
                                                WHERE ats.sport_id = {sportId}
                                                ", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athletes.Add(new()
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    CountryName = reader.GetString(2),
                    Sports = reader.GetString(3)
                });
            }
            _connection.Close();
            return athletes;
        }

        public List<AthleteModel> GetFilteredByCountryData(int countryId)
        {
            List<AthleteModel> athletes = new();
            _connection.Open();
            using var command = new SqlCommand($@"
                                                SELECT a.name, a.surname, c.country_name, STRING_AGG(s.sport_type, ', ') AS 'sports'
                                                FROM athletes a
                                                JOIN athletes_sports ats
                                                ON a.id = ats.athlete_id
                                                JOIN countries c
                                                ON a.country_id = c.country_id
                                                JOIN sports s
                                                ON ats.sport_id = s.sport_id
                                                WHERE c.country_id = {countryId}
                                                GROUP BY a.name, a.surname, c.country_name
                                                ", _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athletes.Add(new()
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    CountryName = reader.GetString(2),
                    Sports = reader.GetString(3)
                });
            }
            _connection.Close();
            return athletes;
        }

        public List<AthleteModel> GetTeamSportData(int isTeamActivity)
        {
            List<AthleteModel> athletes = new();
            string condition;
            if (isTeamActivity == 1)
            {
                condition = "1";
            }
            else
            {
                condition = "0";
            }
            
            _connection.Open();
            using var command = new SqlCommand($@"
                                                SELECT a.name, a.surname, c.country_name, STRING_AGG(s.sport_type, ', ') AS 'sports'
                                                FROM athletes a
                                                JOIN athletes_sports ats
                                                ON a.id = ats.athlete_id
                                                JOIN countries c
                                                ON a.country_id = c.country_id
                                                JOIN sports s
                                                ON ats.sport_id = s.sport_id
                                                WHERE s.team_activity = {condition}
                                                GROUP BY a.name, a.surname, c.country_name"
                                                , _connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athletes.Add(new()
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    CountryName = reader.GetString(2),
                    Sports = reader.GetString(3)
                });
            }
            _connection.Close();
            return athletes;
        }

            public List<AthleteModel> GetSortedData(int sortBy)
            {
                List<AthleteModel> athletes = new();
                _connection.Open();
                using var command = new SqlCommand($@"
                                                SELECT a.name, a.surname, c.country_name, STRING_AGG(s.sport_type, ', ')
                                                FROM athletes a
                                                JOIN countries c
                                                ON a.country_id = c.country_id
                                                JOIN athletes_sports ats
                                                ON a.id = ats.athlete_id
                                                JOIN sports s
                                                ON ats.sport_id = s.sport_id
                                                GROUP BY a.name, a.surname, c.country_name
                                                ORDER BY {sortBy}
                                                ", _connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    athletes.Add(new()
                    {
                        Name = reader.GetString(0),
                        Surname = reader.GetString(1),
                        CountryName = reader.GetString(2),
                        Sports = reader.GetString(3)
                    });
                }
                _connection.Close();
                return athletes;
            }
    }
}
