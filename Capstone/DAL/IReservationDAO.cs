using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{

    public interface IReservationDAO
    {
        int MakeAReservation(int site, string campersName, DateTime fromDate, DateTime toDate);

       
    }
   
}
