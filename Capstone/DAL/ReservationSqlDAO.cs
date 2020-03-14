using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Capstone.DAL
{
    public class ReservationSqlDAO : IReservationDAO
    {
        private string connectionString;

        // Single Parameter Constructor
        public ReservationSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public int MakeAReservation(int site, string campersName, DateTime fromDate, DateTime toDate)
        {
            int reservationId = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = connection.CreateCommand();

                command.CommandText = @"INSERT INTO reservation (site_id, name, from_date, to_date) 
                                                                       VALUES (@site_id, @name, @from_date, @to_date);
                                                                        SELECT SCOPE_IDENTITY()";
                

                command.Parameters.AddWithValue("@site_id", site);
                command.Parameters.AddWithValue("@name", campersName);
                command.Parameters.AddWithValue("@from_date", fromDate);
                command.Parameters.AddWithValue("@to_date", toDate);
             
                connection.Open();

                reservationId = Convert.ToInt32(command.ExecuteScalar());

            }
            return reservationId;

        }

    }
}
