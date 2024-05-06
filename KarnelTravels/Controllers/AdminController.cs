using KarnelTravels.Models;
using KarnelTravels.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Text.RegularExpressions;

namespace KarnelTravels.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _env;

        //Consolidate the constructors into one
        public AdminController(IWebHostEnvironment environment,KarnelTravelsContext context, ILogger<AdminController> logger, IWebHostEnvironment env)
        {
            _environment = environment;
            _context = context;
            _logger = logger;
            _env = env;

        }
        //SITEMAP - a quick routing for debugging and quick access
        public IActionResult Help()
        {
            return View("Sitemap");
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult LoginAdmin(string Username , string Password)
        {
            AdminLogin adminLogin = new AdminLogin(_context);
            var user = adminLogin.Users();
            foreach(var item in user)
            {
                string id =  item.Id.ToString();
                if(item.UserName == Username && item.PassWord == Password)
                {
                    HttpContext.Session.SetString("admin", id);
                    return Redirect("/admin/AdminProfile");
                }
                else
                {
                    return Redirect("/admin/login");
                }
            }
            return View();
        }

        

        public IActionResult AdminTourPackage()
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            GetAllPack getAllPack = new GetAllPack();

            var Pack = tourPackageRepository.GetAllPackage();
            getAllPack.Packages = Pack;
            return View(getAllPack);
        }
        public IActionResult ViewCreateTourPackage(int getid)
        {
            
            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            ImageRepository imageRepository = new ImageRepository(_context);

            var list = spotRepository.GetAllSpot();
            getHotel.tblSpots = list;
            return View(getHotel);
        }
        /*[HttpPost]*/
       
        public IActionResult CreateTourPackage(Packages model , IFormFile FileImg)
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            TblTourPackage tblTourPackage = new TblTourPackage();
            var val = Request.Form["mydata"].ToString();
            string fileName = DateTime.Now.Ticks + FileImg.FileName;
            string uploadPath = Path.Combine(_env.WebRootPath, "image", "tblTour_Packages");

            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                FileImg.CopyTo(stream);
            }
            if (val != null)
            {
                tblTourPackage.StartDate = model.StartDate;
                tblTourPackage.EndDate = model.EndDate;
                tblTourPackage.Name = model.Name;
                tblTourPackage.TotalPrice = model.TotalPrice;
                tblTourPackage.Description = val;
                tblTourPackage.SportId = model.spot;
                tblTourPackage.ImageLinkId = 0;
                _context.TblTourPackages.Add(tblTourPackage);
                _context.SaveChanges();
            }
            return Redirect("/admin/AdminTourPackage");
        }
        public IActionResult ViewEditAdminTourPackage()
        {
            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);    
            int id = int.Parse(Request.Query["idt"]);
           var pack = tourPackageRepository.GetPackageById(id);
            ViewBag.pack = pack;

            
/*            ImageRepository imageRepository = new ImageRepository(_context);
*/
            var list = spotRepository.GetAllSpot();
            getHotel.tblSpots = list;
            return View(getHotel);
        }
        public IActionResult EditAdminTourPackage(TourPackage model, IFormFile FileImg)
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            TblTourPackage tblTourPackage = new TblTourPackage();
            var val = Request.Form["mydata"].ToString();
            int a = model.PackageId;
            /*var ls = travelRepository.GetTravelById(a);*/
            string deletepath = Path.Combine(_env.WebRootPath, "image", "tblTour_Packages");
            /*string fileDelete = Path.Combine(deletepath, ls.);*/
            /*System.IO.File.Delete(fileDelete);*/
            string fileName = DateTime.Now.Ticks + FileImg.FileName;
            string uploadPath = Path.Combine(_env.WebRootPath, "image", "tblTour_Packages");

            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                FileImg.CopyTo(stream);
            }
            if (val != null)
            {
                tblTourPackage.PackageId = a;
                tblTourPackage.SportId = model.SportId;
                tblTourPackage.StartDate = model.StartDate;
                tblTourPackage.EndDate = model.EndDate;
                tblTourPackage.Description = val;
                tblTourPackage.Name = model.Name;
                tblTourPackage.TotalPrice = model.TotalPrice;
                tblTourPackage.ImageLinkId = 0;
                tourPackageRepository.EditPackage(a, tblTourPackage);

            }
            return Redirect("/admin/AdminTourPackage");
        }
        public IActionResult DeleteAdminPackage()
        {
            ITourPackageRepository tourPackage = new ITourPackageRepository(_context);

            int id = int.Parse(Request.Query["idt"]);
            tourPackage.DeletePack(id);
            return Redirect("/admin/AdminTourPackage");
        }
        public IActionResult ViewUploadImageHotel()
        {
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            var list = hotelRepository.GetAllHR();
            getHotel.HrCategories = list;
            return View(getHotel);
            
        }
        public IActionResult Upload(List<IFormFile> files , int objectid , string objectname)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = DateTime.Now.Ticks + file.FileName;
                    string uploadPath = Path.Combine(_env.WebRootPath, "image", "Hotel_Restaurant");

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    TblImageUrl tblImage = new TblImageUrl();
                    tblImage.Url = fileName;
                    tblImage.Description = "upload";
                    tblImage.ObjectId = objectid;
                    tblImage.UrlObject = objectname;
                    _context.TblImageUrls.Add(tblImage);
                    _context.SaveChanges();
                    
                }

            }

            return Redirect("/admin/ViewUploadImageHotel");
        }
        public IActionResult AdminTransportView()
        {
            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getCar_Plane_Train = new GetCar_Plane_Train();
            var tran = travelRepository.GetAllTran();
            
            getCar_Plane_Train.All = tran;
            
            return View(getCar_Plane_Train);
        }

        public IActionResult ViewCreateAdminTransport(int getid)
        {


            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getCar_Plane_Train = new GetCar_Plane_Train();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spot = spotRepository.GetAllSpot();
            var transpot = travelRepository.GetAllTransportation();
            getCar_Plane_Train.Transportations = transpot;
            getCar_Plane_Train.Spots = spot;
            return View(getCar_Plane_Train);
        }

        public IActionResult CreateAdminTransport(IFormFile FileImg , TranSpot model)
        {
            TblTravel tblTravel = new TblTravel();
            string fileName = DateTime.Now.Ticks + FileImg.FileName;
            string uploadPath = Path.Combine(_env.WebRootPath, "image", "Travel");

            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                FileImg.CopyTo(stream);
            }
            var val = Request.Form["mydata"].ToString();
            if (val != null)
            {
                tblTravel.SpotDeparture = model.spot;
                tblTravel.SpotDestination = model.spot1;
                tblTravel.Name = model.Name;
                tblTravel.Price = model.Price;
                tblTravel.Status = "true";
                tblTravel.Description = val;
                tblTravel.TransCategoryId = model.Tran;
                _context.TblTravels.Add(tblTravel);
                _context.SaveChanges();
            }
            return Redirect("/admin/AdminTransportView");
        }

        public IActionResult ViewEditAdminTransport()
        {
            int id = int.Parse(Request.Query["idt"]);
            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getCar_Plane_Train = new GetCar_Plane_Train();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spot = spotRepository.GetAllSpot();
            var transpot = travelRepository.GetAllTransportation();
            var travel = travelRepository.GetTravelById(id);
            getCar_Plane_Train.Transportations = transpot;
            getCar_Plane_Train.Spots = spot;
            
            ViewBag.Travel = travel;
            return View(getCar_Plane_Train);
        }

        public IActionResult EditAdminTransport(IFormFile FileImg, TranSpot model)
        {
            ITravelRepository travelRepository = new ITravelRepository(_context);
            TblTravel tblTravel = new TblTravel();
            var val = Request.Form["mydata"].ToString();
            int a = model.id;
            var ls = travelRepository.GetTravelById(a);
            string deletepath = Path.Combine(_env.WebRootPath, "image", "Travel");
            /*string fileDelete = Path.Combine(deletepath, ls.);*/
            /*System.IO.File.Delete(fileDelete);*/
            string fileName = DateTime.Now.Ticks + FileImg.FileName;
            string uploadPath = Path.Combine(_env.WebRootPath, "image", "Travel");

            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                FileImg.CopyTo(stream);
            }
            if(val != null)
            {
                tblTravel.Name = model.Name;
                tblTravel.Description = val;
                tblTravel.SpotDeparture = model.spot;
                tblTravel.SpotDestination = model.spot1;
                tblTravel.Price = model.Price;
                tblTravel.Status = "true";
                tblTravel.ImageLinkId = null;
                tblTravel.TransCategoryId = model.Tran;
                travelRepository.EditTravel(a, tblTravel);

            }

            return Redirect("/admin/AdminTransportView");
        }

        public IActionResult DeleteAdminTransport()
        {
            int id = int.Parse(Request.Query["idt"]);
            ITravelRepository travelRepository = new ITravelRepository(_context);
            travelRepository.DeleteTravel(id);
            return Redirect("/admin/AdminTransportView");
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
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            GetHotel_Res_Re getHotel_Res_Re = new GetHotel_Res_Re();
            var all = hotelRepository.GetAllHotel_Res_Re();
            
            getHotel_Res_Re.All = all;
            return View(getHotel_Res_Re);

        }
        public IActionResult ViewCreateAdminHotel(int getid )
        {
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            ImageRepository imageRepository = new ImageRepository(_context);
            var image = imageRepository.GetAllRes(getid);
            var list = hotelRepository.GetAllHR();
            var spot1 = spotRepository.GetAllSpot();
            getHotel.tblSpots = spot1;
            getHotel.HrCategories = list;
            getHotel.HrImages = image;
            return View(getHotel);
            
        }
        public IActionResult CreateAdminHotel(HotelRestaurant model,IFormFile FileImg, string editor)
        {
            TblHotelRestaurant tblHotelRestaurant = new TblHotelRestaurant();
            string fileName = DateTime.Now.Ticks + FileImg.FileName;
            string uploadPath = Path.Combine(_env.WebRootPath, "image", "Hotel_Restaurant");

            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                FileImg.CopyTo(stream);
            }
            var val = Request.Form["mydata"].ToString();
            if (val != null)
            {
                tblHotelRestaurant.Name = model.Name;
                tblHotelRestaurant.Price = model.Price;
                tblHotelRestaurant.SpotId = model.spot;
                tblHotelRestaurant.Status = "true";
                tblHotelRestaurant.Description = val;
                tblHotelRestaurant.CatId = model.catid;
                tblHotelRestaurant.Imglink = fileName;
                tblHotelRestaurant.ImageLinkId = 1;
                _context.TblHotelRestaurants.Add(tblHotelRestaurant);
                _context.SaveChanges();

            }
            return Redirect("/admin/AdminHotelView");
        }

        

        public IActionResult ViewEditAdminHotel()
        {
            int q = int.Parse(Request.Query[("idt")]);
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            GetHotel_Res_Re getHotel_Res_Re = new GetHotel_Res_Re();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            var editht = hotelRepository.GetTblHotelRestaurantById(q);
            var list = hotelRepository.GetAllHR();
            var spot1 = spotRepository.GetAllSpot();
            getHotel.tblSpots = spot1;
            getHotel.HrCategories = list;
            ViewBag.editht = editht;
            return View(getHotel);
        }

        public IActionResult EditAdminHotel(HotelRestaurant model , IFormFile FileImg )
        {
            IHotelRepository hotelRepository = new IHotelRepository( _context);
            TblHotelRestaurant tblHotelRestaurant = new TblHotelRestaurant();
            var val = Request.Form["mydata"].ToString();
            int a = model.Hrid;
            var ls = hotelRepository.GetTblHotelRestaurantById(a);
            string deletepath = Path.Combine(_env.WebRootPath, "image", "Hotel_Restaurant");
            string fileDelete = Path.Combine(deletepath, ls.Imglink);
            System.IO.File.Delete(fileDelete);
            string fileName = DateTime.Now.Ticks + FileImg.FileName;
            string uploadPath = Path.Combine(_env.WebRootPath, "image", "Hotel_Restaurant");

            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                FileImg.CopyTo(stream);
            }
            if(val != null)
            {
                tblHotelRestaurant.Name = model.Name;
                tblHotelRestaurant.Price = model.Price;
                tblHotelRestaurant.SpotId = model.spot;
                tblHotelRestaurant.Status = "true";
                tblHotelRestaurant.Description = val;
                tblHotelRestaurant.CatId = model.catid;
                tblHotelRestaurant.Imglink = fileName;
                tblHotelRestaurant.ImageLinkId = 1;
                hotelRepository.EditHotelRestaurant(a, tblHotelRestaurant);
                
            }
            return Redirect("/admin/AdminHotelview");

        }

        public IActionResult DeleteAdminHotel()
        {
            int id = int.Parse(Request.Query["idt"]);
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            hotelRepository.DeleteHotel(id);
            return Redirect("/admin/AdminHotelView");
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
        public IActionResult AdminNewsView()
        {
            var news = _context.TblNews.ToList();
            return View("AdminNewsView", news);
        }
        // [HttpGet] to return selection list of newsobject, news objectname to creating a new News//
        public IActionResult CreateNews()
        {
            var model = new CreateNewsViewModel
            {
                //News= new TblNews(),
                News = new TblNewsWithImageUrls(),
                NewsObjectList = new SelectList(new List<string>
                {
                    "Hotel_Restaurant",
                    "Travel",
                    "Tourist_Place",
                    "Tour_Package"
                    }),
                StatusList = new SelectList(new List<string> { "Active", "Deactive" })
            };

            // Optionally, preload ObjectNameList for a default NewsObject if desired
            if (model.NewsObjectList.Any())
            {
                var defaultNewsObject = "Hotel_Restaurant"; // Example default
                model.ObjectNameList = new SelectList(GetObjectNamesFor(defaultNewsObject), "Id", "Name");
            }

            return View(model);
        }
        //[HttpPost]
        //public IActionResult CreateNews(CreateNewsViewModel model)
        //{
        //    //remove validation of those list//
        //    ModelState.Remove("StatusList");
        //    ModelState.Remove("NewsObjectList");
        //    ModelState.Remove("ObjectNameList");

        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var entry in ModelState)
        //        {
        //            if (entry.Value.Errors.Count > 0)
        //            {
        //                // This will log the key of the model state entry and the error message
        //                _logger.LogError($"ModelState error for {entry.Key}: {entry.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()}");
        //            }
        //        }
        //        return View(model); // Return the view with errors displayed
        //    }
        //    //create new news and assign values form model to it & save to database//
        //    try
        //    {
        //        var news = new TblNews
        //        {
        //            ObjectId = model.News.NewsItem.ObjectId,
        //            Description = model.News.NewsItem.Description,
        //            Status = model.News.NewsItem.Status,
        //            NewsObject = model.News.NewsItem.NewsObject,
        //            HotNews = model.News.NewsItem.HotNews,
        //            Date = DateTime.Now, // Assigning the current date and time
        //            NewsDetail = model.News.NewsItem.NewsDetail
        //        };

        //        _context.TblNews.Add(news); // Correct usage: adding to the DbSet

        //        _context.SaveChanges(); // Save changes in the database

        //        return RedirectToAction("AdminNewsView", "Admin"); // Redirecting after successful operation
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error occurred: {ex.Message}");
        //        return View(model);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateNews(CreateNewsViewModel model, List<IFormFile> files)
        {
            // Remove validation of those lists
            ModelState.Remove("StatusList");
            ModelState.Remove("NewsObjectList");
            ModelState.Remove("ObjectNameList");
            ModelState.Remove("News.ImageUrls");
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        // Log the key of the model state entry and the error message
                        _logger.LogError($"ModelState error for {entry.Key}: {entry.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()}");
                    }
                }
                return View(model); // Return the view with errors displayed
            }

            // Create a new news item and assign values from the model to it
            try
            {
                var news = new TblNews
                {
                    ObjectId = model.News.NewsItem.ObjectId,
                    Description = model.News.NewsItem.Description,
                    Status = model.News.NewsItem.Status,
                    NewsObject = model.News.NewsItem.NewsObject,
                    HotNews = model.News.NewsItem.HotNews,
                    Date = DateTime.Now, // Assigning the current date and time
                    NewsDetail = model.News.NewsItem.NewsDetail
                };

                _context.TblNews.Add(news); // Add to the DbSet

                await _context.SaveChangesAsync(); // Save changes in the database

                // Check and save uploaded files
                if (files != null && files.Count > 0)
                {
                    var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "img", news.NewsObject);
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            var fileName = Path.GetFileName(formFile.FileName);
                            var filePath = Path.Combine(uploadsFolderPath, fileName);

                            // Save the file to the specified folder
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            // Add file info to the database if required
                            var imageUrl = new TblImageUrl
                            {
                                //Url = Path.Combine("img", news.NewsObject, fileName), // Store the relative path
                                Url = fileName,
                                ObjectId = news.ObjectId,
                                UrlObject = news.NewsObject,
                                Description = fileName
                            };
                            _context.TblImageUrls.Add(imageUrl);
                        }
                    }

                    await _context.SaveChangesAsync(); // Save all image URLs to the database
                }

                return RedirectToAction("AdminNewsView", "Admin"); // Redirecting after successful operation
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred: {ex.Message}");
                return View(model);
            }
        }

        //get object names by cases of news object selection as "hotel_restaurant" or "travel" or tourist_place" or "tour_package"//
        private IEnumerable<ObjectNameItem> GetObjectNamesFor(string newsObject)
        {
            switch (newsObject)
            {
                case "Hotel_Restaurant":
                    return _context.TblHotelRestaurants.Select(x => new ObjectNameItem { Id = x.HrId, Name = x.Name }).ToList();
                case "Travel":
                    return _context.TblTravels.Select(x => new ObjectNameItem { Id = x.TravelId, Name = x.Name }).ToList();
                case "Tourist_Place":
                    return _context.TblTouristPlaces.Select(x => new ObjectNameItem { Id = x.Id, Name = x.Name }).ToList();
                case "Tour_Package":
                    return _context.TblTourPackages.Select(x => new ObjectNameItem { Id = x.PackageId, Name = x.Name }).ToList();
                default:
                    return new List<ObjectNameItem>(); // Return an empty list if the case is unknown
            }
        }
        // get objectnames(like hotel's name, touristplace's name, travel's name..etc in json//
        [HttpGet]
        public IActionResult GetObjectNames(string newsObject)
        {
            var objectNames = GetObjectNamesFor(newsObject);
            return Json(objectNames);
        }

        // GET: Admin/Edit a specific news item
        public async Task<IActionResult> EditNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.TblNews.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
          
            var viewModel = new CreateNewsViewModel
            {
                News = await ConvertToNewsWithImages(news),  // Correct usage with 'await'

            StatusList = new SelectList(new List<string> { "Active", "Deactive" }, news.Status),
                NewsObjectList = new SelectList(new List<string>
            {
                "Hotel_Restaurant",
                "Travel",
                "Tourist_Place",
                "Tour_Package"
            }, news.NewsObject),
                ObjectNameList = new SelectList(GetObjectNamesFor("Hotel_Restaurant"), "Id", "Name", news.ObjectId)
            };

            return View(viewModel);
        }

        // POST: Admin/Edit a specific news item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNews(int id, CreateNewsViewModel model, List<IFormFile> files)
        {
            ModelState.Remove("StatusList");
            ModelState.Remove("NewsObjectList");
            ModelState.Remove("ObjectNameList");
            ModelState.Remove("News.ImageUrls");

            if (id != model.News.NewsItem.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newsToUpdate = await _context.TblNews.FindAsync(id);
                    if (newsToUpdate == null)
                    {
                        return NotFound();
                    }

                    newsToUpdate.ObjectId = model.News.NewsItem.ObjectId;
                    newsToUpdate.Description = model.News.NewsItem.Description;
                    newsToUpdate.Status = model.News.NewsItem.Status;
                    newsToUpdate.NewsObject = model.News.NewsItem.NewsObject;
                    newsToUpdate.HotNews = model.News.NewsItem.HotNews;
                    newsToUpdate.Date = DateTime.Now; // changing time when updating news too.
                    newsToUpdate.NewsDetail = model.News.NewsItem.NewsDetail;

                    _context.Update(newsToUpdate);
                    await _context.SaveChangesAsync();

                    //---------------upload more files into tblImageUrl by object if any---//
                    if (files != null && files.Count > 0)
                    {
                        var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "img", newsToUpdate.NewsObject);
                        if (!Directory.Exists(uploadsFolderPath))
                        {
                            Directory.CreateDirectory(uploadsFolderPath);
                        }

                        foreach (var formFile in files)
                        {
                            if (formFile.Length > 0)
                            {
                                var fileName = Path.GetFileName(formFile.FileName);
                                var filePath = Path.Combine(uploadsFolderPath, fileName);

                                // Save the file to the specified folder
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await formFile.CopyToAsync(stream);
                                }

                                // Add file info to the database if required
                                var imageUrl = new TblImageUrl
                                {
                                    //Url = Path.Combine("img", news.NewsObject, fileName), // Store the relative path
                                    Url = fileName,
                                    ObjectId = newsToUpdate.ObjectId,
                                    UrlObject = newsToUpdate.NewsObject,
                                    Description = fileName
                                };
                                _context.TblImageUrls.Add(imageUrl);
                            }
                        }

                        await _context.SaveChangesAsync(); // Save all image URLs to the database
                    }
                    }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(model.News.NewsItem.NewsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(AdminNewsView));
            }

            // Reload dropdowns if needed
            model.StatusList = new SelectList(new List<string> { "Active", "Deactive" }, model.News.NewsItem.Status);
            model.NewsObjectList = new SelectList(new List<string>
        {
            "Hotel_Restaurant",
            "Travel",
            "Tourist_Place",
            "Tour_Package"
        }, model.News.NewsItem.NewsObject);
            model.ObjectNameList = new SelectList(GetObjectNamesFor("Hotel_Restaurant"), "Id", "Name", model.News.NewsItem.ObjectId);

            return View(model);
        }

        private bool NewsExists(int id)
        {
            return _context.TblNews.Any(e => e.NewsId == id);
        }

        // GET: Admin/DeleteNews
        public async Task<IActionResult> DeleteNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.TblNews
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: Admin/DeleteNews
        [HttpPost, ActionName("DeleteNews")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.TblNews.FindAsync(id);
            if (news != null)
            {
                _context.TblNews.Remove(news);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(AdminNewsView));  // Redirect to the list view or appropriate view
        }

        //-- convert a news to newswithimages--------------------//
        public async Task<TblNewsWithImageUrls> ConvertToNewsWithImages(TblNews news)
        {
            if (news == null) return null;

            var imageUrls = await _context.TblImageUrls
                .Where(i => i.ObjectId == news.ObjectId && i.UrlObject == news.NewsObject)  
                .ToListAsync();

            var newsWithImages = new TblNewsWithImageUrls
            {
                NewsItem = news,
                ImageUrls = imageUrls
            };

            return newsWithImages;
        }
        //--------------get images by objectId-----------------//
        [HttpGet]
        public async Task<IActionResult> GetExistingImages( string newsObject, int objectId)
        {
            try
            {
                var imageUrls = await _context.TblImageUrls
                    .Where(i => i.ObjectId == objectId && i.UrlObject == newsObject)
                    .ToListAsync(); // Correctly awaited

                return Json(imageUrls.Select(img => new { url = img.Url }));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //---------upload more images when creating news------------------//

        //[HttpPost]
        //public async Task<IActionResult> UploadImages(List<IFormFile> files, string newsObject, int objectId)
        //{
        //    if (files == null || files.Count == 0)
        //    {
        //        return BadRequest("No files uploaded.");  // Or return to the form with an error message
        //    }

        //    long size = files.Sum(f => f.Length);

        //    var filePath = Path.Combine(_environment.WebRootPath, "img", newsObject);
        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var fileName = Path.GetFileName(formFile.FileName);
        //            var fullPath = Path.Combine(filePath, fileName);

        //            try
        //            {
        //                if (!Directory.Exists(filePath))
        //                {
        //                    Directory.CreateDirectory(filePath);
        //                }

        //                using (var stream = new FileStream(fullPath, FileMode.Create))
        //                {
        //                    await formFile.CopyToAsync(stream);
        //                }

        //                var imageUrl = new TblImageUrl
        //                {
        //                    UrlObject = newsObject,
        //                    Url = fullPath,  // Consider storing only the relative path or just the filename
        //                    ObjectId = objectId,
        //                    Description = fileName
        //                };
        //                _context.TblImageUrls.Add(imageUrl);
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log the error (consider using ILogger)
        //                // Return or handle the error appropriately
        //                return StatusCode(500, "Internal server error: " + ex.Message);
        //            }
        //        }
        //    }

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions from DB saving, e.g., log and return an error message
        //        return StatusCode(500, "Database save error: " + ex.Message);
        //    }

        //    return RedirectToAction("Success");
        //}

    }
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
