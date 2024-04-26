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
        //SITEMAP - a quick routing for debugging and quick access
        public IActionResult Help()
        {
            return View("Admin/Sitemap");
        }
        //---------------------------------------------------------------------------

        //--ROUTINGS FOR REGULAR REDIRECT--
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminTransportView()
        {
            return View("Admin/AdminTransportView");
        }
        public IActionResult AdminTourView()
        {
            return View("Admin/AdminTourView");
        }
        public IActionResult AdminSightView()
        {
            return View("Admin/AdminSightView");
        }
        public IActionResult AdminSpotView()
        {
            return View("Admin/AdminSpotView");
        }
        public IActionResult AdminHotelView()
        {
            return View("Admin/AdminHotelView");
        }
        public IActionResult FeedbackOnObj()
        {
            return View("Admin/FeedbackOnObj");
        }
        public IActionResult FeedbackOnComp()
        {
            return View("Admin/FeedbackOnComp");
        }
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
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AdminProfile()
        {
            return View("Admin/AdminProfile");
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Accessing Privacy page.");
            return View();
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
