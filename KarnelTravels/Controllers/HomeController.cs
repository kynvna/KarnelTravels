using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Ensure ILogger is accessible
using System.Diagnostics;

namespace KarnelTravels.Controllers
{
    public class HomeController : Controller
    {
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<HomeController> _logger;

        // Consolidate the constructors into one
        public HomeController(KarnelTravelsContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        // SITEMAP - a quick routing for debugging and quick access
        //public IActionResult Help()
        //{
        //    return View("Admin/Sitemap");
        //}
        //---------------------------------------------------------------------------

        //--ROUTINGS FOR REGULAR REDIRECT--
        public IActionResult Index()
        {
            return View();
        }
        //public IActionResult AdminTransportView()
        //{
        //    return View("Admin/AdminTransportView");
        //}
        //public IActionResult AdminTourView()
        //{
        //    return View("Admin/AdminTourView");
        //}
        //public IActionResult AdminSightView()
        //{
        //    return View("Admin/AdminSightView");
        //}
        //public IActionResult AdminSpotView()
        //{
        //    return View("Admin/AdminSpotView");
        //}
        //public IActionResult AdminHotelView()
        //{
        //    return View("Admin/AdminHotelView");
        //}
        //public IActionResult FeedbackOnObj()
        //{
        //    return View("Admin/FeedbackOnObj");
        //}
        //public IActionResult FeedbackOnComp()
        //{
        //    return View("Admin/FeedbackOnComp");
        //}
        public IActionResult ProductView()
        {
            return View("User/ProductView");
        }
        public IActionResult AboutUsView()
        {
            return View("User/AboutUsView");
        }
        public IActionResult TravellingHotelView()
        {
            return View("User/TravellingHotelView");
        }
        public IActionResult TravellingRestaurantView()
        {
            return View("User/TravellingRestaurantView");
        }
        public IActionResult TravellingSightView()
        {
            return View("User/TravellingSightView");
        }
        public IActionResult TravellingPackageView()
        {
            return View("User/TravellingPackageView");
        }
        public IActionResult TravellingTourView()
        {
            return View("User/TravellingTourView");
        }
        public IActionResult TravellingTransportView()
        {
            return View("User/TravellingTransportView");
        }
        public IActionResult NewsView()
        {
            return View("User/NewsView");
        }
        public IActionResult FeedbackOnCompany()
        {
            return View("User/FeedbackOnCompany");
        }
        public IActionResult Privacy()
        {
            _logger.LogInformation("Accessing Privacy page.");
            return View();
        }
        // This will go with an ID of a news - After which will be loaded to form the correspond news which is stored in a database
        // NewsDetail/id
        public IActionResult NewsDetail()
        {
            return View("User/NewsDetail");
        }
        // This will go with an ID of a tour in selection of the user - After which will be loaded to form
        // the correspond item (tour, transport, etc..) which is stored in a database
        // TourDetails/id
        public IActionResult TourDetails()
        {
            return View("User/TourDetails");
        }
        public IActionResult SightDetails()
        {
            return View("User/SightDetails");
        }
        public IActionResult HotelDetails()
        {
            return View("User/HotelDetails");
        }
        public IActionResult TransportDetails()
        {
            return View("User/TransportDetails");
        }
        public IActionResult RestaurantDetails()
        {
            return View("User/RestaurantDetails");
        }
        public IActionResult AdvancedSearch()
        {
            return View("User/AdvancedSearch");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An error occurred.");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //-------------------------------




    }
}
