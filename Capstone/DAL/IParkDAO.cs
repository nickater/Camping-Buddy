using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.DAL
{
    public interface IParkDAO
    {
        // returns all parks
        IList<Park> GetParks();

        IList<Park> GetParksInfo(string parkSelection);
    }
}
