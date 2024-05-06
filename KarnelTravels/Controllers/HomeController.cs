using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Ensure ILogger is accessible
using System.Diagnostics;
using System.Linq;
using KarnelTravels.Repository;

namespace KarnelTravels.Controllers
{
    public class HomeController : Controller
    {
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        // Consolidate the constructors into one
        public HomeController(KarnelTravelsContext context, ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _env = env;
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
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            GetHotel_Res_Re getHotel_Res_Re = new GetHotel_Res_Re();
            var all = hotelRepository.GetAllHotel_Res_Re();
            var hotel = hotelRepository.GetAllHotel();
            var res = hotelRepository.GetAllRestaurant();
            var re = hotelRepository.GetAllResort();
            getHotel_Res_Re.All = all;
            getHotel_Res_Re.HotelS = hotel;
            getHotel_Res_Re.Restaurants = res;
            getHotel_Res_Re.Resortt = re;
            
            return View("User/TravellingHotelView",getHotel_Res_Re);
        }

        public IActionResult TravellingHotelViewSearch(string keyWord)
        {
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            if (string.IsNullOrEmpty(keyWord))
            {
                return View("User/TravellingHotelView");
            }
            else
            {
                SearchViewModel searchViewModel = new SearchViewModel();
                var q = hotelRepository.SearchHotel(keyWord);
                searchViewModel.HotelRestaurants = q;
                return View("User/TravellingHotelViewSearch", searchViewModel);

            }

        }
        public IActionResult TravellingRestaurantView()
        {
            return View("User/TravellingRestaurantView");
        }

        public IActionResult TravellingPackageView()
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            GetAllPack getAllPack = new GetAllPack();
            var all = tourPackageRepository.GetAllPackage();
            getAllPack.Packages = all;
            return View("User/TravellingPackageView", getAllPack);
        }
        public IActionResult TravellingPackageViewSearch(string keyWord)
        {
            if (keyWord == null)
            {
                return Redirect("User/TravellingPackageView");
            }
            else
            {
                SearchViewModel searchViewModel = new SearchViewModel();
                ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
                var pack = tourPackageRepository.SearchTourPackages(keyWord);
                searchViewModel.ToursPackage = pack;
                return View("User/TravellingPackageViewSearch", searchViewModel);
            }

        }

        public IActionResult TravellingTourView()
        {

            GetAllTour getAllTour = new GetAllTour();
            ITouristPlaceRepository touristPlaceRepository = new ITouristPlaceRepository(_context);

            var all = touristPlaceRepository.GetAllTour();
            getAllTour.ToursPlace = all;
            return View("User/TravellingTourView", getAllTour);
        }

        public IActionResult TravellingTourViewSearch(string keyWord)
        {
            if (keyWord == null)
            {
                return Redirect("User/TravellingTourView");
            }
            else
            {
                SearchViewModel searchViewModel = new SearchViewModel();
                ITouristPlaceRepository touristPlaceRepository = new ITouristPlaceRepository(_context);
                var q = touristPlaceRepository.SearchTouristPlace(keyWord);
                searchViewModel.Tours = q;
                return View("User/TravellingTourViewSearch", searchViewModel);
            }

        }

        public IActionResult AdminUp()
        {
            return View();
        }



        public IActionResult Upload(List<IFormFile> files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = DateTime.Now.Ticks + file.FileName;
                    string uploadPath = Path.Combine(_env.WebRootPath, "Content", "Upload");

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    TblImageUrl tblImage = new TblImageUrl();
                    tblImage.Url = fileName;
                    tblImage.Description = "upload";
                    _context.TblImageUrls.Add(tblImage);
                    _context.SaveChanges();
                }

            }

            return View();
        }

        public IActionResult TravellingTransportView()
        {
            GetCar_Plane_Train getTran = new GetCar_Plane_Train();
            ITravelRepository travelRepository = new ITravelRepository(_context);
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spot = spotRepository.GetAllSpot();
            var all = travelRepository.GetAllTrain();
            var car = travelRepository.GetAllCar();
            var plane = travelRepository.GetAllPlane();
            var train = travelRepository.GetAllTrain();
            var Transportation = travelRepository.GetAllTransportation();
            getTran.All = all;
            getTran.Cars = car;
            getTran.Spots = spot;
            getTran.Transportations = Transportation;
            return View("User/TravellingTransportView", getTran);
        }

        public IActionResult TravellingTransportViewSearch(int tran,int spot,int spot1)
        {
            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getTran = new GetCar_Plane_Train();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spotall = spotRepository.GetAllSpot();
            
            var Transportation = travelRepository.GetAllTransportation();
            getTran.Spots = spotall;
            getTran.Transportations = Transportation;
            var q = travelRepository.SpotSearch(tran, spot, spot1);
            getTran.All = q;
            return View("User/TravellingTransportViewSearch", getTran);
        }
        
        public IActionResult Privacy()
        {
            _logger.LogInformation("Accessing Privacy page.");
            return View();
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
        public IActionResult FeedbackOnCompany()
        {
            return View("User/FeedbackOnCompany");
        }
        
        // This will go with an ID of a news - After which will be loaded to form the correspond news which is stored in a database
        // NewsDetail/id
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

        public IActionResult PackageDetails()
        {
            int id = int.Parse(Request.Query["idt"]);
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            var pack = tourPackageRepository.GetPackageById(id);
            ViewBag.pack=pack;

            return View("User/PackageDetails");
        }

        public IActionResult HotelDetails()
        {
            int id = int.Parse(Request.Query["idt"]);
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            
            var Hotel = hotelRepository.GetTblHotelRestaurantById(id);
            ViewBag.Hotel = Hotel;
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
        // This will go with an ID of a news - After which will be loaded to form the correspond news which is stored in a database
        // NewsDetail/id
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An error occurred.");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //-------------------------------




    }
}


