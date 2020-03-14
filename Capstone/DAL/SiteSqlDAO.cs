using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAO : ISiteDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public SiteSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public IList<Site> GetSites(string campgroundId, DateTime fromDate, DateTime toDate)
        {
            

            IList<Site> sites = new List<Site>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"SELECT TOP 5 site_number, max_occupancy, accessible, max_rv_length, utilities, c.daily_fee
                                                      FROM site s JOIN campground c ON s.campground_id = c.campground_id
                                                      WHERE s.site_id NOT IN (SELECT r.site_id FROM reservation r WHERE r.from_date < @to_date AND r.to_date > @from_date) AND s.campground_id = @campground_id", connection);


                connection.Open();

                command.Parameters.AddWithValue("@from_date", fromDate);
                command.Parameters.AddWithValue("@to_date", toDate);
                command.Parameters.AddWithValue("@campground_id", campgroundId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    sites.Add(new Site
                    {
                        SiteNumber = Convert.ToInt32(reader["site_number"]),
                        MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]),
                        Accessible = Convert.ToBoolean(reader["accessible"]),
                        MaxRvLength = Convert.ToInt32(reader["max_rv_length"]),
                        Utilities = Convert.ToBoolean(reader["utilities"]),
                        DailyFee =  Convert.ToDouble(reader["daily_fee"])

                    });
                }
            }

            return sites;
        }
    }
}
