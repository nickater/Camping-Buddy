using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;

namespace Capstone.DAL
{
    public class CapstoneCLI
    {
        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        private string selection;
        string campgroundId;
        DateTime fromDate;
        DateTime toDate;
        public string camperName;
        public int siteSelection;

        public CapstoneCLI(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, ISiteDAO siteDAO, IReservationDAO reservationDAO)
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
        }

        public void RunCLI()
        {

            Restart();

            GetSites(campgroundId, fromDate, toDate);

            PrintHeader();


        }
        private void Restart()
        {
            Console.Clear();
            PrintHeader();
            PrintMenu();
            selection = Console.ReadLine();
            Console.WriteLine();
            GetParksInfo(selection);
            CampgroundMenu(selection);
            SiteOptions();



            //selection = Console.ReadLine();
        }
        private void GetParks()
        {
            IList<Park> parks = parkDAO.GetParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine(park.ParkID.ToString().PadRight(10).PadLeft(5) + park.Name.PadRight(40));
                }


            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }
        private void GetParksInfo(string parkSelection)
        {
            IList<Park> parks = parkDAO.GetParksInfo(parkSelection);

            if (parks.Count > 0)
            {
                Console.WriteLine("Park Information Screen");

                foreach (Park park in parks)
                {
                    Console.WriteLine(park.Name);
                    Console.WriteLine("Location:" + park.Location);
                    Console.WriteLine("Established:" + park.EstablishDate);
                    Console.WriteLine("Area:" + park.Area);
                    Console.WriteLine("Annual Visitors" + park.Visitors);
                    Console.WriteLine();
                    Console.WriteLine(park.Description);
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }

        private void GetCampgrounds(string parkId)
        {

            IList<Campground> campgrounds = campgroundDAO.GetCampgrounds(parkId);

            if (campgrounds.Count > 0)
            {
                Console.WriteLine("ID".PadRight(10) + "NAME".PadRight(20) + "OPEN".PadRight(15) + "CLOSES".PadRight(10) + "DAILY FEE".PadRight(10));
                Console.WriteLine("----------------------------------------------------------------");
                foreach (Campground campground in campgrounds)
                {
                    Console.WriteLine(campground.CampgroundId.ToString().PadRight(10) + campground.Name.PadRight(20) + campground.OpenFromMm.ToString().PadRight(15) + campground.OpenToMm.ToString().PadRight(10) + campground.DailyFee.ToString().PadRight(10));
                }
                //GetSites(selection)
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
        }
        private void SiteOptions()
        {
            Console.WriteLine();
            Console.WriteLine("Selct A Command");
            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Return to Previous Screen");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    SiteMenu();


                    break;
                case "2":
                    Console.Clear();
                    PrintHeader();
                    GetParks();
                    break;
            }
        }


        public void SiteMenu()
        {
            campgroundId = CampgroundIdForReservation();
            fromDate = ReservationFromDate();
            toDate = ReservationToDate();
            IList<Site> eligibleSites = siteDAO.GetSites(campgroundId, fromDate, toDate);
            GetSites(campgroundId, fromDate, toDate);
        }
        private string CampgroundIdForReservation()
        {
            Console.WriteLine();
            Console.Write("Which campground (enter 0 to cancel)?  ");
            string campgroundId = Console.ReadLine();
            if (campgroundId == "0")
            {
                Restart();
            }
            return campgroundId;
        }
        private DateTime ReservationFromDate()
        {
            Console.Write("What is the arrival date? ex. 'YYYY-MM-DD' ");
            string fromDateStr = Console.ReadLine();
            DateTime fromDate = Convert.ToDateTime(fromDateStr);
            return fromDate;
        }
        private DateTime ReservationToDate()
        {
            Console.Write("What is the departure date? ex. 'YYYY-MM-DD' ");
            string toDateStr = Console.ReadLine();
            DateTime toDate = Convert.ToDateTime(toDateStr);
            return toDate;

        }



        private void GetSites(string campgroundId, DateTime fromDate, DateTime toDate)
        {
            IList<Site> sites = siteDAO.GetSites(campgroundId, fromDate, toDate);

            if (sites.Count > 0)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine();
                Console.WriteLine("Site No.".PadRight(10) + "Max Occup.".PadRight(14) + "Accessible?".PadRight(15) + "Max RV Length".PadRight(15) + "Utility".PadRight(10) + "Cost".PadRight(10));
                Console.WriteLine("--------------------------------------------------------------------------");
                foreach (Site site in sites)
                {
                    Console.WriteLine(site.SiteNumber.ToString().PadRight(10) + site.MaxOccupancy.ToString().PadRight(14) + site.Accessible.ToString().PadRight(15) + site.MaxRvLength.ToString().PadRight(15) + site.Utilities.ToString().PadRight(10) + "$" + (site.DailyFee * (toDate - fromDate).TotalDays));
                }
                Console.WriteLine();
                SiteSelector();
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.Read();
        }
        private void SiteSelector()
        {
            Console.WriteLine();
            Console.WriteLine("Which site should be reserved (enter 0 to cancel)");
            selection = Console.ReadLine();
            if (selection == "0")
            {
                Restart();
            }
            else
            {
                siteSelection = Convert.ToInt32(selection);
                GetName();
            }
        }


        private void GetName()
        {
            Console.WriteLine("What name should the reservation be made under?");
            camperName = Console.ReadLine();
            Console.Clear();
            PrintHeader();      
            Console.WriteLine("Your reservation was successful with an ID of " + MakeReservation(siteSelection, camperName, fromDate, toDate));
            EndingSequence();
        }
        private void EndingSequence()
        {
            Console.WriteLine();
            Console.WriteLine("Thanks for camping with us! Please select if you'd like to make another reservation, or quit.");
            Console.WriteLine("1) Make another reservation");
            Console.WriteLine("2) Quit");
            selection = Console.ReadLine();
            RestartOrQuit(selection);
        }
        private void RestartOrQuit(string selection)
        {
            if(selection == "1")
            {
                Restart();
            }
            else if(selection == "2")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Make a valid selection");
                
            }

        }

        private int MakeReservation(int site, string campersName, DateTime fromDate, DateTime toDate)
        {
            int reservationId = reservationDAO.MakeAReservation(site, campersName, fromDate, toDate);

            return reservationId;
        }

        private void CampgroundMenu(string ParkId)
        {
            Console.WriteLine("Select a Command");
            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("3) Return to Previous Screen");
            string selection = Console.ReadLine();
            switch (selection)
            {
                case "1":
                    PrintHeader();
                    GetCampgrounds(ParkId);
                    break;
                case "2":
                    SiteMenu();
                    break;
                case "3":
                    Restart();
                    break;

            }


        }

        private void PrintHeader()
        {
            Console.Clear();
            Console.WriteLine(@"  _____                      _               ____            _     _       ");
            Console.WriteLine(@" / ____|                    (_)             |  _ \          | |   | |      ");
            Console.WriteLine(@"| |     __ _ _ __ ___  _ __  _ _ __   __ _  | |_) |_   _  __| | __| |_   _ ");
            Console.WriteLine(@"| |    / _` | '_ ` _ \| '_ \| | '_ \ / _` | |  _ <| | | |/ _` |/ _` | | | |");
            Console.WriteLine(@"| |___| (_| | | | | | | |_) | | | | | (_| | | |_) | |_| | (_| | (_| | |_| |");
            Console.WriteLine(@" \_____\__,_|_| |_| |_| .__/|_|_| |_|\__, | |____/ \__,_|\__,_|\__,_|\__, |");
            Console.WriteLine(@"                      | |             __/ |                           __/ |");
            Console.WriteLine(@"                      |_|            |___/                           |___/ ");
            Console.WriteLine();
            Console.WriteLine();
        }
        private void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("Select a Park for Further Detail");
            GetParks();

            Console.WriteLine("Q".PadRight(10).PadLeft(5) + "Quit".PadRight(40));
        }
        private void ErrorMessage()
        {
            Console.WriteLine("Sorry, try again");
        }
    }
}
