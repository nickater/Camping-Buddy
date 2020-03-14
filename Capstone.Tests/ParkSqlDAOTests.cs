using Microsoft.Data.Tools.Schema.Sql.UnitTesting;
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
    public class ParkSqlDATests : SqlDatabaseSetup
    {
        private ParkSqlDAO dao;

        [TestInitialize]
        public override void Setup()
        {
            
            base.Setup();
            dao = new ParkSqlDAO(ConnectionString);


        }

        [TestMethod]
        public void easy_park_default_values_test()
        {
            IList<Park> actualParks = dao.GetParks();
            Assert.AreEqual("Test Park", actualParks[0].Name);
            Assert.AreEqual("Test Description", actualParks[0].Description);
            Assert.AreEqual(DateTime.Parse("12/17/1990 12:00:00 AM"), actualParks[0].EstablishDate);
            Assert.AreEqual(43210, actualParks[0].Area);
            Assert.AreEqual("Test State", actualParks[0].Location);
            Assert.AreEqual(105, actualParks[0].Visitors);
        }

        [TestMethod]
        public void return_all_parks_test()
        {
            IList<Park> actualParks = dao.GetParks();

            Assert.AreEqual(1, actualParks.Count);
        }
    }
}
