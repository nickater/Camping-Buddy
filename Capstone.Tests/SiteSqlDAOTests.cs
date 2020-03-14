using Capstone.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace Capstone.Tests
{
    [TestClass]
    public class SiteSqlDAOTests : SqlDatabaseSetup
    {
        private SiteSqlDAOTests dao;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();


            // Execute the script
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO site (campground_id, site_number, max_occupancy, accessible, max_rv_length, utilities)
                                VALUES (1, 1, 20, 1, 40, 1)", connection);
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }



        //[TestMethod]
        //public void site_default_values_test()
        //{
        //    IList<Site> actualSites = dao.GetSites();
        //}

        //[TestMethod]
        //public void return_all_sites_test()
        //{
        //    IList<Site> actualSites = dao.GetSites();

        //    Assert.AreEqual(1, actualSites.Count);
        //}
    }
}
