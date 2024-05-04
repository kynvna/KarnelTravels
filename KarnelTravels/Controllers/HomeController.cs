using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Ensure ILogger is accessible
using System.Diagnostics;
using System.Linq;

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
       
        //---------------------------------------------------------------------------

        //--ROUTINGS FOR REGULAR REDIRECT--
        public IActionResult Index()
        {
            return View();
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

        public IActionResult NewsView(string filterType = "HotNews", int page = 1, int pageSize = 6)
        {
            ViewBag.SelectedTab = filterType ?? "HotNews"; // Ensure the default selected tab is HotNews
            var viewModel = GetFilteredNewsViewModel(filterType, page, pageSize);
            return View("User/NewsView", viewModel);
        }


        private NewsViewModel GetFilteredNewsViewModel(string filterType, int page, int pageSize)
        {
            IQueryable<TblNews> query;

            switch (filterType)
            {
                case "HotNews":
                    query = _context.TblNews.Where(n => n.HotNews == 1);
                    break;
                case "Hotel_Restaurant":
                    query = _context.TblNews.Where(n => n.NewsObject == "Hotel_Restaurant");
                    break;
                case "Travel":
                    query = _context.TblNews.Where(n => n.NewsObject == "Travel");
                    break;
                case "Tourist_Place":
                    query = _context.TblNews.Where(n => n.NewsObject == "Tourist_Place");
                    break;
                case "Tour_Package":
                    query = _context.TblNews.Where(n => n.NewsObject == "Tour_Package");
                    break;
                default:
                    query = _context.TblNews;
                    break;
            }

            var totalNews = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalNews / pageSize);

            // Fetch image URLs for each news item
            var newsItems = query.OrderBy(n => n.Date) // Assuming there's a Date field
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .Select(news => new TblNewsWithImageUrls
                                 {
                                     NewsItem = news,
                                     ImageUrls = _context.TblImageUrls
                                                      .Where(i => i.ObjectId == news.ObjectId && i.UrlObject == news.NewsObject)
                                                      .ToList()
                                 })
                                 .ToList(); // Execute the query

            return new NewsViewModel
            {
                TotalNews = totalNews,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                News = newsItems
            };
        }

        //-------process for a detail of news ----------------------//

        public IActionResult NewsDetail(int id)
        {
            // First, get the news item based on the provided ID.
            var newsItem = _context.TblNews.FirstOrDefault(n => n.NewsId == id);

            // Check if the news item exists; if not, return a NotFound result.
            if (newsItem == null)
            {
                return NotFound();
            }

            // Get related news items
            var relatedNewsItems = _context.TblNews
                .Where(n => n.NewsObject == newsItem.NewsObject && n.ObjectId == newsItem.ObjectId && n.NewsId != id)
                .ToList();

            // Manually collect images for each related news item
            var relatedNewsWithImages = relatedNewsItems.Select(news => new TblNewsWithImageUrls
            {
                NewsItem = news,
                ImageUrls = _context.TblImageUrls
                            .Where(i => i.ObjectId == news.ObjectId && i.UrlObject == news.NewsObject)
                            .ToList()
            }).ToList();

            // Now, create a new TblNewsWithImageUrls object that includes the news item and its associated image URLs.
            var newsWithImages = new TblNewsWithImageUrls
            {
                NewsItem = newsItem,
                ImageUrls = _context.TblImageUrls
                            .Where(i => i.ObjectId == newsItem.ObjectId && i.UrlObject == newsItem.NewsObject)
                            .ToList()
            };

            // Prepare the ViewModel
            var viewModel = new NewsDetailViewModel
            {
                MainNewsWithImages = newsWithImages,
                RelatedNewsWithImages = relatedNewsWithImages // This is now a list of TblNewsWithImageUrls
            };

            // Return the view with the constructed ViewModel.
            return View("User/NewsDetail", viewModel);
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
