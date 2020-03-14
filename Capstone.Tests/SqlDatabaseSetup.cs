using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Transactions;
using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Capstone.Tests
{
    [TestClass]
    public abstract class SqlDatabaseSetup
    {
        private IConfigurationRoot config;


        private TransactionScope transaction;

        protected IConfigurationRoot Config
        {
            get
            {
                if (config == null)
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json");

                    config = builder.Build();
                }
                return config;
            }
        }

        protected string ConnectionString
        {
            get
            {
                return Config.GetConnectionString("Project");
            }
        }

        [TestInitialize]
        public virtual void Setup()
        {
            transaction = new TransactionScope();

            string sql = @"DELETE FROM reservation;
                           DELETE FROM site;
                           DELETE FROM campground;
                           DELETE FROM park;
                           SET IDENTITY_INSERT park ON
                           INSERT INTO park(park_id, name, location, establish_date, area, visitors, description)
                           VALUES(1,'Test Park', 'Test State', '1990-12-17', 43210, 105, 'Test Description');
                           SET IDENTITY_INSERT park OFF
                           SET IDENTITY_INSERT campground ON
                           INSERT INTO campground(campground_id, park_id, name, open_from_mm, open_to_mm, daily_fee)
                           VALUES(1, 1, 'Test Name', 1, 12, 30.00);
                           SET IDENTITY_INSERT campground OFF
                           SET IDENTITY_INSERT site ON
                           INSERT INTO site(site_id,campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                           VALUES(1, 1, 1, 6, 1, 20, 1);
                           SET IDENTITY_INSERT site OFF
                           SET IDENTITY_INSERT reservation ON
                           INSERT INTO reservation(reservation_id, site_id, name, from_date, to_date)
                           VALUES(1, 1, 'Test Reservation', '2020-01-01', '2020-01-10');
                           SET IDENTITY_INSERT reservation OFF";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, connection);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }
    }
}
