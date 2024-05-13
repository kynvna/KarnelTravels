using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Ensure ILogger is accessible
using System.Diagnostics;
using System.Linq;
using KarnelTravels.Repository;
using System.Security.AccessControl;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;


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

        [HttpGet]
        public IActionResult TravellingSightView(string searchString)
        {
            Dictionary<int, List<TouristPlaceViewModel>> touristPlaceViewModelsBySpot = new Dictionary<int, List<TouristPlaceViewModel>>();
            try
            {
                // Lấy danh sách các điểm du lịch
                List<TblTouristPlace> touristPlaces = _context.TblTouristPlaces.ToList();

                // Lặp qua từng điểm du lịch để nhóm chúng theo SpotId và thực hiện tìm kiếm
                foreach (var touristPlace in touristPlaces)
                {
                    var spotId = touristPlace.SportId;
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == spotId);

                    if (spot != null)
                    {
                        // Filter based on search string for place name or spot name
                        if (string.IsNullOrEmpty(searchString) ||
                            touristPlace.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                            spot.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        {
                            var imageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == touristPlace.Id && i.UrlObject == "TblTourist_Place");
                            var touristPlaceViewModel = new TouristPlaceViewModel
                            {
                                Id = touristPlace.Id,
                                Name = touristPlace.Name,
                                Description = touristPlace.Description,
                                Status = touristPlace.Status,
                                ImageUrl = imageUrl?.Url,
                                Namespot = spot.Name
                            };

                            // Kiểm tra nếu SpotId đã tồn tại trong từ điển
                            if (touristPlaceViewModelsBySpot.ContainsKey((int)spotId))
                            {
                                // Nếu đã tồn tại, thêm điểm du lịch vào danh sách tương ứng
                                touristPlaceViewModelsBySpot[(int)spotId].Add(touristPlaceViewModel);
                            }
                            else
                            {
                                // Nếu chưa tồn tại, tạo mới danh sách và thêm điểm du lịch vào
                                touristPlaceViewModelsBySpot[(int)spotId] = new List<TouristPlaceViewModel> { touristPlaceViewModel };
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View("User/TravellingSightView", touristPlaceViewModelsBySpot);
        }
        public IActionResult SightDetails(int id)
        {
            try
            {
                var touristPlace = _context.TblTouristPlaces.FirstOrDefault(tp => tp.Id == id);
                if (touristPlace != null)
                {
                    var imageUrls = _context.TblImageUrls
                        .Where(i => i.ObjectId == touristPlace.Id && i.UrlObject == "TblTourist_Place")
                        .Select(i => i.Url)
                        .ToList();
                    var viewModel = new TouristPlaceUserViewModel
                    {
                        Id = touristPlace.Id,
                        Name = touristPlace.Name,
                        Description = touristPlace.Description,
                        ImageUrl = imageUrls
                    };
                    return View("User/SightDetails", viewModel);
                }
                else
                {
                    TempData["errorMessage"] = "Tourist place not found.";
                    return RedirectToAction("TravellingSightView"); // Redirect to a view displaying all tourist places
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred while retrieving tourist place details: {ex.Message}";
                return RedirectToAction("TravellingSightView"); // Redirect to a view displaying all tourist places
            }
        }
        public IActionResult TravellingHotelView(int page = 1, int pageSize = 6)
        {
            string a = "Hotel_Restaurant";
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            ViewHotelUser viewHotelAll = new ViewHotelUser();
            var all = hotelRepository.GetAllHotel_Res_Re(a, page, pageSize);
            var hotel = hotelRepository.GetAllHotel(a, page, pageSize);
            var res = hotelRepository.GetAllRestaurant(a, page, pageSize);
            var re = hotelRepository.GetAllResort(a, page, pageSize);
            /*getHotel_Res_Re.All = all;
            getHotel_Res_Re.HotelS = hotel;
            getHotel_Res_Re.Restaurants = res;
            getHotel_Res_Re.Resortt = re;*/
            ViewBag.all = all;
            ViewBag.hotel = hotel;
            ViewBag.res = res;
            ViewBag.re = re;
            return View("User/TravellingHotelView");
        }

        public IActionResult TravellingHotelViewSearch(string keyWord)
        {
            string a = "Hotel_Restaurant";
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            if (string.IsNullOrEmpty(keyWord))
            {
                return View("User/TravellingHotelView");
            }
            else
            {
                SearchViewModel searchViewModel = new SearchViewModel();
                var q = hotelRepository.SearchHotel(keyWord, a);
                searchViewModel.HotelRestaurants = q;
                return View("User/TravellingHotelViewSearch", searchViewModel);

            }

        }
        public IActionResult TravellingRestaurantView()
        {
            return View("User/TravellingRestaurantView");
        }
        public IActionResult TravellingPackageView(int page = 1, int pageSize = 6)
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            GetAllPack getAllPack = new GetAllPack();
            string a = "Tour_Package";
            var all = tourPackageRepository.GetAllPackImg(a, page, pageSize);

            return View("User/TravellingPackageView", all);
        }
        public IActionResult TravellingPackageViewSearch(string keyWord)
        {
            string a = "Tour_Package";
            if (keyWord == null)
            {
                return Redirect("User/TravellingPackageView");
            }
            else
            {

                ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
                SearchViewModel searchViewModel = new SearchViewModel();
                var pack = tourPackageRepository.SearchTourPackages(keyWord, a);
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

        public IActionResult TravellingTransportView(int page = 1, int pageSize = 6)
        {
            string a = "Travel";
            GetCar_Plane_Train getTran = new GetCar_Plane_Train();
            ITravelRepository travelRepository = new ITravelRepository(_context);
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spot = spotRepository.GetAllSpot();
            var all = travelRepository.GetAllTravleImg(a, page, pageSize);
            var car = travelRepository.GetAllCar(a);
            var plane = travelRepository.GetAllPlane(a);
            var train = travelRepository.GetAllTrain(a);
            var Transportation = travelRepository.GetAllTransportation();

            getTran.Cars = car;
            getTran.Spots = spot;
            getTran.Planes = plane;
            getTran.Trains = train;

            getTran.Transportations = Transportation;
            ViewBag.all = all;
            return View("User/TravellingTransportView", getTran);
        }

        public IActionResult TravellingTransportViewSearch(int tran, int spot, int spot1)
        {

            string a = "Travel";
            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getTran = new GetCar_Plane_Train();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spotall = spotRepository.GetAllSpot();

            var Transportation = travelRepository.GetAllTransportation();
            getTran.Spots = spotall;
            getTran.Transportations = Transportation;
            var q = travelRepository.SpotSearch(a, tran, spot, spot1);
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

        public IActionResult PackageDetails()
        {
            int id = int.Parse(Request.Query["idt"]);
            string a = "Tour_Package";
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            ImageRepository imageRepository = new ImageRepository(_context);
            ViewPackageUser viewPackageUser = new ViewPackageUser();
            var img = imageRepository.GetAllImg(id, a);
            var pack = tourPackageRepository.GetTblPackImgById(id, a);


            viewPackageUser.packageImg = pack;
            viewPackageUser.Images = img;

            return View("User/PackageDetails", viewPackageUser);
        }

        public IActionResult HotelDetails()
        {
            string a = "Hotel_Restaurant";
            int id = int.Parse(Request.Query["idt"]);
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            ViewHotelUser viewHotelUser = new ViewHotelUser();
            ImageRepository imageRepository = new ImageRepository(_context);
            var img = imageRepository.GetAllImg(id, a);
            var hotels = hotelRepository.GetTblHotelImgById(id, a);
            viewHotelUser.Hotels = hotels;
            viewHotelUser.Images = img;
            return View("User/HotelDetails", viewHotelUser);
        }
        public IActionResult TransportDetails()
        {
            string a = "Travel";
            int id = int.Parse(Request.Query["idt"]);
            ITravelRepository travelRepository = new ITravelRepository(_context);
            ViewTravelUser viewTravelUser = new ViewTravelUser();
            ImageRepository imgRepository = new ImageRepository(_context);
            var img = imgRepository.GetAllImg(id, a);
            var travel = travelRepository.GetTblTravelImgById(id, a);
            viewTravelUser.travelImg = travel;
            viewTravelUser.tblImageUrls = img;
            return View("User/TransportDetails", viewTravelUser);
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



        [HttpPost]
        public async Task<JsonResult> SubmitFeedback(FeedbackViewModel model, string recaptchaResponse)
        {
            // Verify the reCAPTCHA response first
            var client = new HttpClient();
            var recaptchaVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
            var recaptchaSecretKey = "6LfY39gpAAAAAJcc1wxVCXEA--zxygaAsKLZ_kf8";  // Replace with your actual secret key
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("secret", recaptchaSecretKey),
        new KeyValuePair<string, string>("response", recaptchaResponse)
    });

            var response = await client.PostAsync(recaptchaVerificationUrl, content);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var captchaResult = JsonConvert.DeserializeObject<GoogleRecaptchaVerification>(jsonResult);

            if (!captchaResult.Success || captchaResult.Score < 0.5)  // Assuming a threshold score of 0.5
            {
                return Json(new { success = false, message = "CAPTCHA validation failed or score too low" });
            }

            // Proceed with storing feedback if CAPTCHA is passed
            if (ModelState.IsValid)
            {
                var feedback = new TblFeedback
                {
                    CustomerId = model.CustomerEmail,
                    Feedback = model.CommentText,
                    FeedbackObject = model.FeedbackObject,
                    ObjectId = model.ObjectId,
                    Status = model.Status,
                    Date = DateTime.Now,
                    Rating = model.Rating,
                    IsRead=model.IsRead
                };
                if (model.FeedbackObject == "Company")
                {
                    feedback.ObjectName = "Karnel"; // for company
                }
                else
                {
                    feedback.ObjectName = "To be defined"; // Adjust accordingly
                }
                _context.TblFeedbacks.Add(feedback);
                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Invalid feedback model state" });
        }

        // Supporting class to handle reCAPTCHA response
        public class GoogleRecaptchaVerification
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("score")]
            public float Score { get; set; }  // Add score property for reCAPTCHA v3

            [JsonProperty("challenge_ts")]
            public DateTime ChallengeTimestamp { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }


        //----------searching news----------------//
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
            // Add sorting by Date in descending order before selecting
            newsQuery = newsQuery.OrderByDescending(n => n.Date);
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




