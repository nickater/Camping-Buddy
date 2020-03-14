using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CampgroundSqlDAO : ICampgroundDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public CampgroundSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public IList<Campground> GetCampgrounds(string parkId)
        {
            IList<Campground> campgrounds = new List<Campground>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(@"SELECT campground_id, name, open_from_mm, open_to_mm, daily_fee 
                                                      FROM campground 
                                                      WHERE park_id = @park_id", connection);
                connection.Open();

                command.Parameters.AddWithValue("@park_id", parkId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    campgrounds.Add(new Campground
                    {
                        CampgroundId = Convert.ToInt32(reader["campground_id"]),
                        Name = reader["name"] as string,
                        OpenFromMm = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(reader["open_from_mm"])),
                        OpenToMm = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(reader["open_to_mm"])),
                        DailyFee = Convert.ToDecimal(reader["daily_fee"])

                    });
                }
            }
            return campgrounds;
        }

    }
}
