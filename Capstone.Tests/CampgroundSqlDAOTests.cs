using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class CampgroundSqlDAOTests : SqlDatabaseSetup
    {
        private CampgroundSqlDAO dao;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new CampgroundSqlDAO(ConnectionString);
            // initialize the values of campground
            // run reader through the SQL command
            // assign values from the reader results

            // Execute the script
            //using (SqlConnection connection = new SqlConnection(ConnectionString))
            //{
            //    SqlCommand cmd = new SqlCommand(
            //        @"INSERT INTO campground (name, open_from_mm, open_to_mm, daily_fee)
            //                           VALUES('Test Name', 1, 12, 30.00);
            //           INSERT INTO site (site", connection);

            //    connection.Open();

            //    cmd.ExecuteNonQuery();
            //}
        }



        [TestMethod]
        public void easy_campground_default_values_test()
        {
            IList<Campground> actualCampgrounds = dao.GetCampgrounds("1");

            Assert.AreEqual("Test Name", actualCampgrounds[0].Name);
            Assert.AreEqual("January", actualCampgrounds[0].OpenFromMm);
            Assert.AreEqual("December", actualCampgrounds[0].OpenToMm);
            Assert.AreEqual(30, actualCampgrounds[0].DailyFee);
        }

        //[TestMethod]
        //public void return_all_campground_test()
        //{

        //    IList<Campground> actualCampgrounds = dao.GetCampgrounds(park_id);

        //    Assert.AreEqual(1, actualCampgrounds.Count);
        //}
    }
}
