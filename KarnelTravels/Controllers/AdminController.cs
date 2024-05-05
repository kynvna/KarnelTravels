using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public IActionResult CreateHotel(TblHotelRestaurant model,List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {


                var catid = 1;

                var hotel = new TblHotelRestaurant
                {
                    CatId = catid,
                    Name = model.Name,
                    Price = model.Price,
                    SpotId = model.SpotId,
                    Status = model.Status,
                    Description = model.Description,

                };
                _context.TblHotelRestaurants.Add(hotel);
                _context.SaveChanges();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string path = _environment.WebRootPath;
                        var filename = file.FileName;

                        string savePath = path + "/img" + "/Hotel_Restaurant/" + filename;
                        var stream = System.IO.File.Create(savePath);
                        file.CopyTo(stream);

                        var url = new TblImageUrl
                        {
                            Description = "hotel",
                            Url = filename,
                            ObjectId = hotel.HrId,
                            UrlObject = "Hotel_Restaurant"
                        };
                        _context.TblImageUrls.Add(url);
                    }
                    
                    _context.SaveChanges();
                }

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
                        Status = hotel.Status,
                        Description = hotel.Description,
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
        public IActionResult EditHotel(TblHotelRestaurant model,List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {


                var catid = 1;

                var hotel = new TblHotelRestaurant
                {
                    HrId = model.HrId,
                    CatId = catid,
                    Name = model.Name,
                    Price = model.Price,
                    SpotId = model.SpotId,
                    Status = model.Status,
                    Description = model.Description,

                };
                _context.TblHotelRestaurants.Update(hotel);
                _context.SaveChanges();
                if (files != null)
                {
                    foreach(var file in files)
                    {
                        string path = _environment.WebRootPath;
                        var filename = file.FileName;

                        string savePath = path + "/img/Hotel_Restaurant/" + filename;

                        var stream = System.IO.File.Create(savePath);
                        file.CopyTo(stream);

                        var existingImageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == hotel.HrId && i.UrlObject == "Hotel_Restaurant");
                        if (existingImageUrl != null)
                        {
                            // Update existing image URL
                            existingImageUrl.Description = "hotel";
                            existingImageUrl.Url = filename;
                            _context.TblImageUrls.Update(existingImageUrl);
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Create new image URL if it doesn't exist
                            var newImageUrl = new TblImageUrl
                            {
                                Description = "hotel",
                                Url = filename,
                                ObjectId = hotel.HrId,
                                UrlObject = "Hotel_Restaurant"
                            };
                            _context.TblImageUrls.Add(newImageUrl);
                            
                        }
                    }
                    _context.SaveChanges();
                }

                // Redirect to a success page or return a success message
                return RedirectToAction("AdminHotel");

            }
            else
            {
                TempData["errorMessage"] = "Model data is not valid!";
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
                    // Retrieve the image URL
                    var imageUrls = _context.TblImageUrls.Where(i => i.ObjectId == id && i.UrlObject == "Hotel_Restaurant").ToList();
                    foreach(var imageUrl in imageUrls)
                    {
                        _context.TblImageUrls.Remove(imageUrl);
                        _context.SaveChanges();
                        // Delete the image file from the server
                        string path = _environment.WebRootPath;
                        var imagePath = path + "/img/Hotel_Restaurant/" + imageUrl.Url;
                        System.IO.File.Delete(imagePath);

                    }

                    // Delete the hotel
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
            List<HRViewModel> resortViewModels = new List<HRViewModel>();
            try
            {
                List<TblHotelRestaurant> resorts = _context.TblHotelRestaurants.Where(r => r.CatId == 2).ToList();
                foreach (var resort in resorts)
                {
                    var spot = _context.TblSpots.FirstOrDefault(s => s.Id == resort.SpotId);
                    if (spot != null)
                    {
                        var imageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == resort.HrId && i.UrlObject == "Hotel_Restaurant");
                        var resortViewModel = new HRViewModel
                        {
                            HrId = resort.HrId,
                            CatId = resort.CatId,
                            Name = resort.Name,
                            SpotName = spot.Name,
                            Price = resort.Price,
                            Description = resort.Description,
                            Status = resort.Status,
                            ImageUrl = imageUrl?.Url // Assign the image URL
                        };
                        resortViewModels.Add(resortViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(resortViewModels);
        }



        [HttpGet]
        public IActionResult CreateResort()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateResort(TblHotelRestaurant model,List <IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var catid = 2; // Assuming CatId 2 represents resorts

                var resort = new TblHotelRestaurant
                {
                    CatId = catid,
                    Name = model.Name,
                    Price = model.Price,
                    SpotId = model.SpotId,
                    Status = model.Status,
                    Description = model.Description,
                };
                _context.TblHotelRestaurants.Add(resort);
                _context.SaveChanges();

                if (files != null)
                {
                    foreach(var file in files)
                    {
                        string path = _environment.WebRootPath;
                        var filename = DateTime.Now.Ticks + file.FileName;

                        string savePath = path + "/img" + "/Hotel_Restaurant/" + filename;
                        var stream = System.IO.File.Create(savePath);
                        file.CopyTo(stream);

                        var url = new TblImageUrl
                        {
                            Description = "resort",
                            Url = filename,
                            ObjectId = resort.HrId,
                            UrlObject = "Hotel_Restaurant"
                        };
                        _context.TblImageUrls.Add(url);
                    }
                    
                    _context.SaveChanges();
                }

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
                        Status = hotel.Status,
                        Description = hotel.Description,
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
        public IActionResult EditResort(TblHotelRestaurant model,List <IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var catid = 2; // Assuming CatId 2 represents resorts

                var resort = new TblHotelRestaurant
                {
                    HrId = model.HrId,
                    CatId = catid,
                    Name = model.Name,
                    Price = model.Price,
                    SpotId = model.SpotId,
                    Status = model.Status,
                    Description = model.Description,
                };
                _context.TblHotelRestaurants.Update(resort);
                _context.SaveChanges();

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        string path = _environment.WebRootPath;
                        var filename = file.FileName;

                        string savePath = path + "/img/Hotel_Restaurant/" + filename;
                        var stream = System.IO.File.Create(savePath);
                        file.CopyTo(stream);
                        var existingImageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == resort.HrId && i.UrlObject == "Hotel_Restaurant");
                        if (existingImageUrl != null)
                        {
                            // Update existing image URL
                            existingImageUrl.Description = "resort";
                            existingImageUrl.Url = filename;
                            _context.TblImageUrls.Update(existingImageUrl);
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Create new image URL if it doesn't exist
                            var newImageUrl = new TblImageUrl
                            {
                                Description = "hotel",
                                Url = filename,
                                ObjectId = resort.HrId,
                                UrlObject = "Hotel_Restaurant"
                            };
                            _context.TblImageUrls.Add(newImageUrl);
                            _context.SaveChanges();
                        }


                    }

                }

                // Redirect to a success page or return a success message
                return RedirectToAction("AdminResort");
            }
            else
            {
                TempData["errorMessage"] = "Model data is not valid!";
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
                    // Retrieve the image URL
                    var imageUrls = _context.TblImageUrls.Where(i => i.ObjectId == id && i.UrlObject == "Hotel_Restaurant").ToList();
                    foreach(var imageUrl in imageUrls)
                    {
                        // Delete the image file from the server
                        _context.TblImageUrls.Remove(imageUrl);
                        _context.SaveChanges();

                        string path = _environment.WebRootPath;
                        var imagePath = path + "/img/Hotel_Restaurant/" + imageUrl.Url;
                        System.IO.File.Delete(imagePath);
                    }

                    // Delete the resort from the database
                    _context.TblHotelRestaurants.Remove(resort);
                    _context.SaveChanges();
                    TempData["successMessage"] = "Resort deleted successfully";
                }
                else
                {
                    TempData["errorMessage"] = $"Resort with ID {id} not found";
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
                        var imageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == t.Id && i.UrlObject == "TblTourist_Place");
                        var TouristPlaceViewModel = new TouristPlaceViewModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                            ImageUrl = imageUrl?.Url,
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
        public IActionResult CreateTouristPlace(TblTouristPlace model, List<IFormFile> files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var TouristPlace = new TblTouristPlace
                    {
                        Name = model.Name,
                        Status = model.Status,
                        SportId = model.SportId,
                        Description = model.Description,

                    };
                    _context.TblTouristPlaces.Add(model);
                    _context.SaveChanges();
                    if (files != null)
                    {
                        string path = _environment.WebRootPath;
                        foreach (var file in files)
                        {
                            var filename = DateTime.Now.Ticks + file.FileName;

                            string savePath = path + "/img" + "/TblTourist_Place/" + filename;
                            var stream = System.IO.File.Create(savePath);
                            file.CopyTo(stream);

                            var url = new TblImageUrl
                            {
                                Description = "Tourist Place",
                                Url = filename,
                                ObjectId = model.Id,
                                UrlObject = "TblTourist_Place"
                            };
                            _context.TblImageUrls.Add(url);

                        }
                       
                        _context.SaveChanges();
                    }

                    TempData["successMessage"] = "Tourist place created successfully";
                    return RedirectToAction("AdminTouristPlace"); // Redirect to a success page 
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
                    var tours = new TblTouristPlace()
                    {
                        Name = tour.Name,
                        Id = tour.Id,
                        Status = tour.Status,
                        SportId = tour.SportId,
                        Description = tour.Description,
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
        public IActionResult EditTouristPlace(TblTouristPlace model,List<IFormFile> files)
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
                    };
                    _context.TblTouristPlaces.Update(tour);
                    _context.SaveChanges();
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            string path = _environment.WebRootPath;
                            var filename = file.FileName;

                            string savePath = path + "/img/TblTourist_Place/" + filename;

                            var stream = System.IO.File.Create(savePath);
                            file.CopyTo(stream);

                            var existingImageUrl = _context.TblImageUrls.FirstOrDefault(i => i.ObjectId == tour.Id && i.UrlObject == "TblTourist_Place");
                            if (existingImageUrl != null)
                            {
                                // Update existing image URL
                                existingImageUrl.Description = "Tourist Place";
                                existingImageUrl.Url = filename;
                                _context.TblImageUrls.Update(existingImageUrl);
                                _context.SaveChanges();
                            }
                            else
                            {
                                // Create new image URL if it doesn't exist
                                var newImageUrl = new TblImageUrl
                                {
                                    Description = "Tourist Place",
                                    Url = filename,
                                    ObjectId = tour.Id,
                                    UrlObject = "TblTourist_Place"
                                };
                                _context.TblImageUrls.Add(newImageUrl);
                            }

                        }
                            _context.SaveChanges();
                        
                    }


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
                    var imageUrls = _context.TblImageUrls.Where(i => i.ObjectId == id && i.UrlObject == "TblTourist_Place").ToList();
                    foreach (var imageUrl in imageUrls)
                    {
                        _context.TblImageUrls.Remove(imageUrl);
                        _context.SaveChanges();
                        string path = _environment.WebRootPath;
                        var imagePath = path + "/img/TblTourist_Place/" + imageUrl.Url;
                        System.IO.File.Delete(imagePath);
                    }
                   

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
