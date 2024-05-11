using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Ensure ILogger is accessible
using System.Diagnostics;
using System.Linq;
using System.Security.AccessControl;

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
                //case "All":
                //    query = _context.TblNews;
                //    break;
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



        //----------submit feedbacks-----------------------//
        [HttpPost]
        public JsonResult SubmitFeedback(FeedbackViewModel model)
        {
            if (ModelState.IsValid)
            {
                var feedback = new TblFeedback
                {
                    CustomerId = model.CustomerEmail,
                    Feedback = model.CommentText,
                    FeedbackObject = model.FeedbackObject,
                    ObjectId = model.ObjectId,
                    Status = model.Status,
                    Date= DateTime.Now,
                    Rating=model.Rating
                    
                };
                if (model.FeedbackObject == "Company")
                {
                    feedback.ObjectName = "Kernal"; // for company
                }
                else 
                {
                    feedback.ObjectName = "To be define"; // Adjust accordingly
                }
                _context.TblFeedbacks.Add(feedback);
                _context.SaveChanges();

                // Return JSON indicating success
                return Json(new { success = true });
            }

            // Return JSON indicating failure
            return Json(new { success = false });
        }

        //-----implement the searching news---------//
        [HttpPost]
        public IActionResult SearchNews(string searchText, int? spotsId)
        {
            // Query and filter the news data
            var newsQuery = _context.TblNews.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                newsQuery = newsQuery.Where(n => n.Description.Contains(searchText) || n.NewsDetail.Contains(searchText));
            }

            if (spotsId.HasValue)
            {
                newsQuery = newsQuery.Where(n =>
                    (n.NewsObject == "Hotel_Restaurant" && _context.TblHotelRestaurants.Any(hr => hr.HrId == n.ObjectId && hr.SpotId == spotsId)) ||
                    (n.NewsObject == "Tourist_Place" && _context.TblTouristPlaces.Any(tp => tp.Id == n.ObjectId && tp.SportId == spotsId)) ||
                    (n.NewsObject == "Tour_Package" && _context.TblTourPackages.Any(tp => tp.PackageId == n.ObjectId && tp.SportId == spotsId)) ||
                    (n.NewsObject == "Travel" && _context.TblTravels.Any(tt => tt.TravelId == n.ObjectId && tt.SpotDestination == spotsId))
                );
            }

            // Select news data with image URLs
            var newsList = newsQuery.Select(news => new TblNewsWithImageUrls
            {
                NewsItem = news,
                ImageUrls = _context.TblImageUrls
                    .Where(i => i.ObjectId == news.ObjectId && i.UrlObject == news.NewsObject)
                    .ToList()
            }).ToList();

            // Return the partial view with filtered data
            return PartialView("User/NewsListPartial", newsList);
        }


        //------get all spots----------------//
        [HttpGet]
        public JsonResult GetAllSpots()
        {
            var spots = _context.TblSpots
                .Select(s => new { s.Id, s.Name }) // Adjust with actual column names
                .ToList();

            return Json(spots); // Return spots data as JSON
        }
    }
}
