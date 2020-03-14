using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.Tests
{
    [TestClass]
    public class ReservationTests : SqlDatabaseSetup
    {
        private ReservationSqlDAO dao;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();
            dao = new ReservationSqlDAO(ConnectionString);


        }

        [TestMethod()]
        public void make_a_new_reservation_test()
        {

            Assert.AreEqual(1, dao.MakeAReservation(1, "Billy", Convert.ToDateTime("2019-01-20"), Convert.ToDateTime("2019-01-27")));
        }

        [TestMethod()]
        public void make_an_invalid_reseveration_test()
        {

        }
    }
}
