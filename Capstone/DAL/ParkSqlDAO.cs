using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAO : IParkDAO
    {
        private const string CmdText = "SELECT park_id, name, location, establish_date, area, visitors, description FROM park ORDER BY name";

        private string connectionString;

        // Single Parameter Constructor
        public ParkSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public IList<Park> GetParks()
        {
            IList<Park> parks = new List<Park>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(CmdText, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    parks.Add(new Park
                    {
                        ParkID = Convert.ToInt32(reader["park_id"]),
                        Name = reader["name"] as string,
                        Location = reader["location"] as string,
                        EstablishDate = Convert.ToDateTime(reader["establish_date"]),
                        Area = Convert.ToInt32(reader["area"]),
                        Visitors = Convert.ToInt32(reader["visitors"]),
                        Description = reader["description"] as string

                    });
                }
            }

            return parks;
        }

        public IList<Park> GetParksInfo(string parkSelection)
        {
            IList<Park> parks = new List<Park>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT park_id, name, location, establish_date, area, visitors, description FROM park WHERE park_id = @parkId", connection);
                connection.Open();

                command.Parameters.AddWithValue("@parkId", parkSelection);

                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    parks.Add(new Park
                    {
                        ParkID = Convert.ToInt32(reader["park_id"]),
                        Name = reader["name"] as string,
                        Location = reader["location"] as string,
                        EstablishDate = Convert.ToDateTime(reader["establish_date"]),
                        Area = Convert.ToInt32(reader["area"]),
                        Visitors = Convert.ToInt32(reader["visitors"]),
                        Description = reader["description"] as string

                    });
                }
            }

            return parks;
        }
    }
}
