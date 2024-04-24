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

        //--ROUTINGS FOR REGULAR REDIRECT--
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ProductView()
        {
            return View();
        }
        public IActionResult AboutUsView()
        {
            return View();
        }
        public IActionResult TravellingHotelView()
        {
            return View();
        }
        public IActionResult TravellingRestaurantView()
        {
            return View();
        }
        public IActionResult TravellingSightView()
        {
            return View();
        }
        public IActionResult TravellingPackageView()
        {
            return View();
        }
        public IActionResult TravellingTourView()
        {
            return View();
        }
        public IActionResult TravellingTransportView()
        {
            return View();
        }
        public IActionResult NewsView()
        {
            return View();
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
