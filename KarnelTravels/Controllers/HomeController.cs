using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Ensure ILogger is accessible
using System.Diagnostics;
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
            return View();
        }
        public IActionResult TravellingSightView()
        {
            return View();
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

        public IActionResult NewsView()
        {
            return View("User/NewsView");
        }
        public IActionResult FeedbackOnCompany()
        {
            return View("User/FeedbackOnCompany");
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("An error occurred.");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //-------------------------------




    }
}
