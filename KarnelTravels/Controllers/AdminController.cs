using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravels.Controllers
{
    public class AdminController : Controller
    {
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _environment;

        //Consolidate the constructors into one
        public AdminController(KarnelTravelsContext context, ILogger<AdminController> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;

        }
        //SITEMAP - a quick routing for debugging and quick access
        public IActionResult Help()
        {
            return View("Admin/Sitemap");
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AdminTransportView()
        {
            return View("AdminTransportView");
        }
        public IActionResult AdminTourView()
        {
            return View("AdminTourView");
        }
        public IActionResult AdminSightView()
        {
            return View("AdminSightView");
        }
        public IActionResult AdminSpotView()
        {
            return View("AdminSpotView");
        }
        public IActionResult AdminHotelView()
        {
            return View("AdminHotelView");
        }
        public IActionResult FeedbackOnObj()
        {
            return View("FeedbackOnObj");
        }
        public IActionResult FeedbackOnComp()
        {
            return View("FeedbackOnComp");
        }
        public IActionResult AdminProfile()
        {
            return View("AdminProfile");
        }





        //-------------------------------Hotel//-------------------------------//-------------------------------



        [HttpGet]
        public IActionResult AdminHotel()
        {
            List<HRViewModel> hotelViewModels = new List<HRViewModel>();
            try
            {
                List<TblHotelRestaurant> hotels = _context.TblHotelRestaurants.Where(r => r.CatId == 1).ToList();
                foreach (var hotel in hotels)
                {
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == hotel.SpotId);
                    if (spot != null)
                    {
                        var hotelViewModel = new HRViewModel
                        {
                            HrId = hotel.HrId,
                            CatId = hotel.CatId,
                            Name = hotel.Name,
                            SpotName = spot.Name,
                            Price = hotel.Price,
                            Description = hotel.Description,
                            ImageLinkId = hotel.ImageLinkId,
                            Imglink = hotel.Imglink,
                            Status = hotel.Status
                        };
                        hotelViewModels.Add(hotelViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(hotelViewModels);
        }


        [HttpGet]
        public IActionResult CreateHotel()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateHotel(CreateHRModel model)
        {

            if (ModelState.IsValid)
            {



                string path = _environment.WebRootPath;
                var ticks = DateTime.Now.Ticks;
                var filename = model.Imglink.FileName;

                string savePath = path + "/image/Hotel" + filename;
                var stream = System.IO.File.Create(savePath);
                model.Imglink.CopyTo(stream);
                var catid = 1;

                var hotel = new TblHotelRestaurant
                {
                    CatId = catid,
                    Name = model.Name,
                    Price = model.Price,
                    SpotId = model.SpotId,
                    Imglink = filename,
                    Status = model.Status,
                    Description = model.Description,
                    ImageLinkId = model.ImageLinkId,

                };
                _context.TblHotelRestaurants.Add(hotel);
                _context.SaveChanges();

                // Redirect to a success page or return a success message
                return RedirectToAction("AdminHotel");




            }
            else
            {
                TempData["errorMessage"] = "Model data is not valid!";
                return View();

            }

        }



        [HttpGet]
        public IActionResult EditHotel(int Id)
        {
            var hotel = _context.TblHotelRestaurants.SingleOrDefault(x => x.HrId == Id);

            try
            {
                if (hotel != null)
                {
                    var HotelView = new TblHotelRestaurant()
                    {
                        HrId=hotel.HrId,
                        Name = hotel.Name,
                        Price = hotel.Price,
                        SpotId = hotel.SpotId,
                        Imglink = hotel.Imglink,
                        Status = hotel.Status,
                        Description = hotel.Description,
                        ImageLinkId = hotel.ImageLinkId,
                        CatId = hotel.CatId,
                    };
                    return View(HotelView);
                }
                else
                {
                    TempData["errorMessage"] = $"Hotel details is not available with the id: {Id}";
                    return RedirectToAction("AdminHotel");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("AdminHotel");
            }

        }



        [HttpPost]
        public IActionResult EditHotel(EditHRModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string path = _environment.WebRootPath;
                    var ticks = DateTime.Now.Ticks;
                    var filename = model.Imglink.FileName;

                    string savePath = path + "/image/Hotel" + filename;
                    var stream = System.IO.File.Create(savePath);
                    model.Imglink.CopyTo(stream);
                    var catid = 1;

                    var hotel = new TblHotelRestaurant
                    {
                        HrId = model.HrId,
                        CatId = catid,
                        Name = model.Name,
                        Price = model.Price,
                        SpotId = model.SpotId,
                        Imglink = filename,
                        Status = model.Status,
                        Description = model.Description,
                        ImageLinkId = model.ImageLinkId,

                    };
                    _context.TblHotelRestaurants.Update(hotel);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Hotel details update successfully";
                    return RedirectToAction("AdminHotel");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }



        [HttpPost]
        public IActionResult DeleteHotel(int id)
        {
            try
            {
                var hotel = _context.TblHotelRestaurants.SingleOrDefault(x => x.HrId == id);
                if (hotel != null)
                {
                    _context.TblHotelRestaurants.Remove(hotel);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Hotel deleted successfully";
                }
                else
                {
                    TempData["errorMessage"] = $"Hotel with ID {id} not found";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Failed to delete hotel: {ex.Message}";
            }

            return RedirectToAction("AdminHotel");
        }







        //-------------------------------Resort//-------------------------------//-------------------------------





        [HttpGet]
        public IActionResult AdminResort()
        {
            List<HRViewModel> resortt = new List<HRViewModel>();
            try
            {
                List<TblHotelRestaurant> resorts = _context.TblHotelRestaurants.Where(r => r.CatId == 2).ToList();
                foreach (var resort in resorts)
                {
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == resort.SpotId);
                    if (spot != null)
                    {
                        var resortViewModel = new HRViewModel
                        {
                            HrId = resort.HrId,
                            CatId = resort.CatId,
                            Name = resort.Name,
                            SpotName = spot.Name,
                            Price = resort.Price,
                            Description = resort.Description,
                            ImageLinkId = resort.ImageLinkId,
                            Imglink = resort.Imglink,
                            Status = resort.Status
                        };
                        resortt.Add(resortViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(resortt);
        }


        [HttpGet]
        public IActionResult CreateResort()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateResort(CreateHRModel model)
        {

            if (ModelState.IsValid)
            {



                string path = _environment.WebRootPath;
                var ticks = DateTime.Now.Ticks;
                var filename = model.Imglink.FileName;

                string savePath = path + "/image/Resort" + filename;
                var stream = System.IO.File.Create(savePath);
                model.Imglink.CopyTo(stream);
                var catid = 2;

                var resort = new TblHotelRestaurant
                {
                    CatId = catid,
                    Name = model.Name,
                    Price = model.Price,
                    SpotId = model.SpotId,
                    Imglink = filename,
                    Status = model.Status,
                    Description = model.Description,
                    ImageLinkId = model.ImageLinkId,

                };
                _context.TblHotelRestaurants.Add(resort);
                _context.SaveChanges();

                // Redirect to a success page or return a success message
                return RedirectToAction("AdminResort");




            }
            else
            {
                TempData["errorMessage"] = "Model data is not valid!";
                return View();

            }

        }



        [HttpGet]
        public IActionResult EditResort(int Id)
        {
            var hotel = _context.TblHotelRestaurants.SingleOrDefault(x => x.HrId == Id);

            try
            {
                if (hotel != null)
                {
                    var ResortView = new TblHotelRestaurant()
                    {
                        Name = hotel.Name,
                        Price = hotel.Price,
                        SpotId = hotel.SpotId,
                        Imglink = hotel.Imglink,
                        Status = hotel.Status,
                        Description = hotel.Description,
                        ImageLinkId = hotel.ImageLinkId,
                        CatId = hotel.CatId,
                        HrId = hotel.HrId,
                    };
                    return View(ResortView);
                }
                else
                {
                    TempData["errorMessage"] = $"Resort details is not available with the id: {Id}";
                    return RedirectToAction("AdminResort");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("AdminResort");
            }

        }
        [HttpPost]
        public IActionResult EditResort(EditHRModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string path = _environment.WebRootPath;
                    var ticks = DateTime.Now.Ticks;
                    var filename = model.Imglink.FileName;

                    string savePath = path + "/image/Resort" + filename;
                    var stream = System.IO.File.Create(savePath);
                    model.Imglink.CopyTo(stream);
                    var catid = 2;

                    var resort = new TblHotelRestaurant
                    {
                        HrId = model.HrId,
                        CatId = catid,
                        Name = model.Name,
                        Price = model.Price,
                        SpotId = model.SpotId,
                        Imglink = filename,
                        Status = model.Status,
                        Description = model.Description,
                        ImageLinkId = model.ImageLinkId,

                    };
                    _context.TblHotelRestaurants.Update(resort);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Resort details update successfully";
                    return RedirectToAction("AdminResort");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }


        [HttpPost]
        public IActionResult DeleteResort(int id)
        {
            try
            {
                var resort = _context.TblHotelRestaurants.SingleOrDefault(x => x.HrId == id);
                if (resort != null)
                {
                    _context.TblHotelRestaurants.Remove(resort);
                    _context.SaveChanges();
                    TempData["successMessage"] = "resort deleted successfully";
                }
                else
                {
                    TempData["errorMessage"] = $"resort with ID {id} not found";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Failed to delete resort: {ex.Message}";
            }

            return RedirectToAction("AdminResort");
        }




        //-------------------------------Spot//-------------------------------//-------------------------------

        [HttpGet]
        public IActionResult AdminSpot()
        {

            List<TblSpot> spot = new List<TblSpot>();
            try
            {
                spot = _context.TblSpots.ToList();
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(spot);
        }






        [HttpGet]
        public IActionResult CreateSpot()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateSpot(TblSpot model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var spot = new TblSpot
                    {
                        Name = model.Name,
                        Id = model.Id,
                        Status = model.Status,
                    };
                    _context.TblSpots.Add(model);
                    _context.SaveChanges();

                    TempData["successMessage"] = "Spot created successfully";
                    return RedirectToAction("AdminSpot"); // Redirect to a success page or any other page
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid!";
                    return View(model); // Return the view with the model if validation fails
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Failed to create spot: {ex.Message}";
                return View(model); // Return the view with the model in case of an exception
            }
        }



        [HttpGet]
        public IActionResult EditSpot(int id)
        {
            try
            {
                var spot = _context.TblSpots.FirstOrDefault(s => s.Id == id);

                if (spot != null)
                {
                    var spots = new TblSpot
                    {
                        Name = spot.Name,
                        Id = spot.Id,
                        Status = spot.Status,
                    };
                    return View(spots);
                }
                else
                {
                    TempData["errorMessage"] = "Spot not found.";
                    return RedirectToAction("AdminSpot");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred while editing spot: {ex.Message}";
                return RedirectToAction("AdminSpot");
            }
        }

        [HttpPost]
        public IActionResult EditSpot(TblSpot model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var spot = new TblSpot
                    {
                        Name = model.Name,
                        Id = model.Id,
                        Status = model.Status,
                    };
                    _context.TblSpots.Update(spot);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Resort details update successfully";
                    return RedirectToAction("AdminSpot");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }



        [HttpPost]
        public IActionResult DeleteSpot(int id)
        {
            try
            {
                var spot = _context.TblSpots.SingleOrDefault(x => x.Id == id);
                if (spot != null)
                {
                    _context.TblSpots.Remove(spot);
                    _context.SaveChanges();
                    TempData["successMessage"] = "spot deleted successfully";
                }
                else
                {
                    TempData["errorMessage"] = $"spot with ID {id} not found";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Failed to delete spot: {ex.Message}";
            }

            return RedirectToAction("AdminSpot");
        }





        //-------------------------------Tourist Place//-------------------------------//-------------------------------


        [HttpGet]
        public IActionResult AdminTouristPlace()
        {

            List<TouristPlaceViewModel> TouristPlace = new List<TouristPlaceViewModel>();
            try
            {
                List<TblTouristPlace> TouristPlaces = _context.TblTouristPlaces.ToList();
                foreach (var t in TouristPlaces)
                {
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == t.SportId);
                    if (spot != null)
                    {
                        var TouristPlaceViewModel = new TouristPlaceViewModel
                        {
                            Id = t.Id,

                            Name = t.Name,
                            Description = t.Description,
                            ImageLinkId = t.ImageLinkId,
                            Status = t.Status,
                            Namespot = spot.Name                   
                        };
                        TouristPlace.Add(TouristPlaceViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(TouristPlace);
        }






        [HttpGet]
        public IActionResult CreateTouristPlace()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTouristPlace(TblTouristPlace model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var TouristPlace = new TblTouristPlace
                    {
                        Name = model.Name,
                        Id = model.Id,
                        Status = model.Status,
                        SportId = model.SportId,
                        Description = model.Description,
                        ImageLinkId = model.ImageLinkId,

                    };
                    _context.TblTouristPlaces.Add(model);
                    _context.SaveChanges();

                    TempData["successMessage"] = "Tourist place created successfully";
                    return RedirectToAction("TouristPlace"); // Redirect to a success page 
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid!";
                    return View(model); // Return the view with the model if validation fails
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Failed to create Tourist place: {ex.Message}";
                return View(model); // Return the view with the model in case of an exception
            }
        }




        [HttpGet]
        public IActionResult EditTouristPlace(int id)
        {
            try
            {
                var tour = _context.TblTouristPlaces.FirstOrDefault(s => s.Id == id);

                if (tour != null)
                {
                    var tours = new TblTouristPlace
                    {
                        Name = tour.Name,
                        Id = tour.Id,
                        Status = tour.Status,
                        SportId = tour.SportId,
                        Description = tour.Description,
                        ImageLinkId = tour.ImageLinkId
                    };
                    return View(tours);
                }
                else
                {
                    TempData["errorMessage"] = "Place not found.";
                    return RedirectToAction("AdminTouristPlace");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Error occurred while editing spot: {ex.Message}";
                return RedirectToAction("AdminTouristPlace");
            }
        }

        [HttpPost]
        public IActionResult EditTouristPlace(TblTouristPlace model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var tour = new TblTouristPlace
                    {
                        Name = model.Name,
                        Id = model.Id,
                        Status = model.Status,
                        SportId = model.SportId,
                        Description = model.Description,
                        ImageLinkId = model.ImageLinkId,
                    };
                    _context.TblTouristPlaces.Update(tour);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Resort details update successfully";
                    return RedirectToAction("AdminTouristPlace");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is not valid!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }



        [HttpPost]
        public IActionResult DeleteTouristPlace(int id)
        {
            try
            {
                var tour = _context.TblTouristPlaces.SingleOrDefault(x => x.Id == id);
                if (tour != null)
                {
                    _context.TblTouristPlaces.Remove(tour);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Place deleted successfully";
                }
                else
                {
                    TempData["errorMessage"] = $"place with ID {id} not found";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Failed to delete tourist place: {ex.Message}";
            }

            return RedirectToAction("AdminTouristPlace");
        }

    }
}
