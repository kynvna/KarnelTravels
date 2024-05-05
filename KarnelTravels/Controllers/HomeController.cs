using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        public IActionResult TravellingRestaurantView()
        {
            return View("User/TravellingRestaurantView");
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
        //public IActionResult Login()
        //{
        //    return View();
        //}
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





        //-------------------------------Hotel & Resort-------------------------------


        [HttpGet]
        public IActionResult TravellingHotelView()
        {
            Dictionary<int, List<HRViewModel>> hotelViewModelsBySpot = new Dictionary<int, List<HRViewModel>>();
            try
            {
                // L?y danh sách khách s?n thu?c CatId 1 ho?c 2
                List<TblHotelRestaurant> hotels = _context.TblHotelRestaurants.Where(r => r.CatId == 1 || r.CatId == 2).ToList();

                // L?p qua t?ng khách s?n ?? nhóm chúng theo SpotId
                foreach (var hotel in hotels)
                {
                    var spotId = hotel.SpotId;
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == spotId);

                    if (spot != null)
                    {
                        var imageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == hotel.HrId && i.UrlObject == "Hotel_Restaurant");
                        var hotelViewModel = new HRViewModel
                        {
                            HrId = hotel.HrId,
                            CatId = hotel.CatId,
                            Name = hotel.Name,
                            SpotName = spot.Name,
                            Price = hotel.Price,
                            Description = hotel.Description,
                            Status = hotel.Status,
                            ImageUrl = imageUrl?.Url // Assign the image URL
                        };

                        // Ki?m tra n?u SpotId ?ã t?n t?i trong t? ?i?n
                        if (hotelViewModelsBySpot.ContainsKey((int)spotId))
                        {
                            // N?u ?ã t?n t?i, thêm khách s?n vào danh sách t??ng ?ng
                            hotelViewModelsBySpot[(int)spotId].Add(hotelViewModel);
                        }
                        else
                        {
                            // N?u ch?a t?n t?i, t?o m?i danh sách và thêm khách s?n vào
                            hotelViewModelsBySpot[(int)spotId] = new List<HRViewModel> { hotelViewModel };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View("User/TravellingHotelView", hotelViewModelsBySpot);
        }


        public IActionResult HotelDetails(int id)
        {
            try
            {
                var Hotel = _context.TblHotelRestaurants.FirstOrDefault(tp => tp.HrId == id);

                if (Hotel != null)
                {
                    var imageUrls = _context.TblImageUrls
    .Where(i => i.ObjectId == Hotel.HrId && i.UrlObject == "Hotel_Restaurant")
    .Select(i => i.Url)
    .ToList();

                    var viewModel = new HRUserViewModel
                    {
                        HrId = Hotel.HrId,
                        Name = Hotel.Name,
                        Description = Hotel.Description,
                        Price = Hotel.Price,
                        ImageUrl = imageUrls


                    };

                    return View("User/HotelDetails", viewModel);
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





        //-------------------------------Tourist Place---------------------------------------------------------------------------------------------


        [HttpGet]
        public IActionResult TravellingSightView()
        {
            Dictionary<int, List<TouristPlaceViewModel>> touristPlaceViewModelsBySpot = new Dictionary<int, List<TouristPlaceViewModel>>();
            try
            {
                // Lấy danh sách các điểm du lịch
                List<TblTouristPlace> touristPlaces = _context.TblTouristPlaces.ToList();

                // Lặp qua từng điểm du lịch để nhóm chúng theo SpotId
                foreach (var touristPlace in touristPlaces)
                {
                    var spotId = touristPlace.SportId;
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == spotId);

                    if (spot != null)
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
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View("User/TravellingSightView",touristPlaceViewModelsBySpot);
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






    }
}


