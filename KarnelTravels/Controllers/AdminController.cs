using KarnelTravels.Models;
using KarnelTravels.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.AccessControl;
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
        //Consolidate the constructors into one
        public AdminController(IWebHostEnvironment environment, KarnelTravelsContext context, ILogger<AdminController> logger, IWebHostEnvironment env)
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

        public IActionResult LoginAdmin(string Username, string Password)
        {
            AdminLogin adminLogin = new AdminLogin(_context);
            var user = adminLogin.Users();
            foreach (var item in user)
            {
                string id = item.Id.ToString();
                if (item.UserName == Username && item.PassWord == Password)
                {
                    HttpContext.Session.SetString("admin", id);
                    return Redirect("/admin/GetAllFeedBacks");
                }
                else
                {
                    return Redirect("/admin/login");
                }
            }
            return View();
        }



        public IActionResult AdminTourPackage(int page = 1, int pageSize = 8)
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            GetAllPack getAllPack = new GetAllPack();
            string a = "Tour_Package";
            var Pack = tourPackageRepository.GetAllPackImg(a, page, pageSize);

            return View(Pack);
        }
        public IActionResult ViewCreateTourPackage(int getid)
        {

            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            ImageRepository imageRepository = new ImageRepository(_context);

            var list = spotRepository.GetAllSpot();
            getHotel.tblSpots = list;
            getHotel.selectObject = new SelectList(new List<string>
                {
                    "Hotel_Restaurant",
                    "Travel",
                    "Tourist_Place",
                    "Tour_Package"
                    });
            return View(getHotel);
        }
        /*[HttpPost]*/

        public IActionResult CreateTourPackage(Packages model, List<IFormFile> FileImg)
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            TblTourPackage tblTourPackage = new TblTourPackage();
            var val = Request.Form["mydata"].ToString();


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
                int newid = _context.SaveChanges();
                if (newid > 0) // Check for successful save
                {
                    var newlyCreatedId = tblTourPackage.PackageId;
                    if (FileImg != null)
                    {
                        foreach (var file in FileImg)
                        {
                            string fileName = DateTime.Now.Ticks + file.FileName;
                            string uploadPath = Path.Combine(_env.WebRootPath, "img", "tblTour_Packages");

                            string filePath = Path.Combine(uploadPath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            TblImageUrl tblImage = new TblImageUrl();
                            tblImage.Url = fileName;
                            tblImage.Description = "upload";
                            tblImage.ObjectId = newlyCreatedId;
                            tblImage.UrlObject = model.selectObject;
                            _context.TblImageUrls.Add(tblImage);
                            _context.SaveChanges();

                        }

                    }// Assuming Id property is auto-generated
                }
            }
            return Redirect("/admin/AdminTourPackage");
        }
        public IActionResult ViewEditAdminTourPackage()
        {
            string a = "Tour_Package";
            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            ImageRepository imageRepository = new ImageRepository(_context);

            int id = int.Parse(Request.Query["idt"]);
            var pack = tourPackageRepository.GetPackageById(id);
            var img = imageRepository.GetAllImg(id, a);
            ViewBag.pack = pack;


            /*            ImageRepository imageRepository = new ImageRepository(_context);
            */
            var list = spotRepository.GetAllSpot();
            getHotel.tblSpots = list;
            getHotel.HrImages = img;
            return View(getHotel);
        }
        public IActionResult EditAdminTourPackage(TourPackage model, List<IFormFile> FileImg)
        {
            ITourPackageRepository tourPackageRepository = new ITourPackageRepository(_context);
            TblTourPackage tblTourPackage = new TblTourPackage();
            var val = Request.Form["mydata"].ToString();
            int newlyCreatedId = model.PackageId;
            /*var ls = travelRepository.GetTravelById(a);*/
            string deletepath = Path.Combine(_env.WebRootPath, "img", "tblTour_Packages");
            /*string fileDelete = Path.Combine(deletepath, ls.);*/
            /*System.IO.File.Delete(fileDelete);*/
          
            if (val != null)
            {
                tblTourPackage.PackageId = newlyCreatedId;
                tblTourPackage.SportId = model.SportId;
                tblTourPackage.StartDate = model.StartDate;
                tblTourPackage.EndDate = model.EndDate;
                tblTourPackage.Description = val;
                tblTourPackage.Name = model.Name;
                tblTourPackage.TotalPrice = model.TotalPrice;
                tblTourPackage.ImageLinkId = 0;
                tourPackageRepository.EditPackage(newlyCreatedId, tblTourPackage);

            }
            if (FileImg != null && model != null)
            {

                foreach (var file in FileImg)
                {
                    string fileName = DateTime.Now.Ticks + file.FileName;
                    string uploadPath = Path.Combine(_env.WebRootPath, "img", "tblTour_Packages");

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    TblImageUrl tblImage = new TblImageUrl();
                    tblImage.Url = fileName;
                    tblImage.Description = "upload";
                    tblImage.ObjectId = newlyCreatedId;
                    tblImage.UrlObject = "tblTour_Packages";
                    _context.TblImageUrls.Add(tblImage);
                    _context.SaveChanges();


                }

            }// Assuming Id property is 
            return Redirect("/admin/AdminTourPackage");
        }

        public IActionResult DeletePackImg(int imageId)
        {
            ImageRepository imageRepository = new ImageRepository(_context);
            // Check if image ID is valid
            if (imageId <= 0)
            {
                return BadRequest("Invalid image ID."); // Return bad request if invalid ID
            }

            // Fetch image details from repository
            var image = imageRepository.GetById(imageId);

            // Check if image exists
            if (image == null)
            {
                return NotFound("Image not found."); // Return not found if image doesn't exist
            }

            // Build deletion path
            string deletePath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant", image.Url);

            try
            {
                // Attempt to delete file
                System.IO.File.Delete(deletePath);

                // Delete image record from database
                imageRepository.DeleteImg(imageId);

                // Return success response (optional)
                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(ex.Message);
            }
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
        public IActionResult Upload(List<IFormFile> files, int objectid, string objectname)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = DateTime.Now.Ticks + file.FileName;
                    string uploadPath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant");

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
        public IActionResult AdminTransportView(int page = 1, int pageSize = 6)
        {
            string a = "Travel";
            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getCar_Plane_Train = new GetCar_Plane_Train();
            var tran = travelRepository.GetAllTravleImg(a, page, pageSize);

            return View(tran);
        }

        public IActionResult ViewCreateAdminTransport(int getid)
        {


            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getCar_Plane_Train = new GetCar_Plane_Train();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            var spot = spotRepository.GetAllSpot();
            var transpot = travelRepository.GetAllTransportation();
            getCar_Plane_Train.selectObject = new SelectList(new List<string>
                {
                    "Hotel_Restaurant",
                    "Travel",
                    "Tourist_Place",
                    "Tour_Package"
                    });
            getCar_Plane_Train.Transportations = transpot;
            getCar_Plane_Train.Spots = spot;
            return View(getCar_Plane_Train);
        }

        public IActionResult CreateAdminTransport(List<IFormFile> FileImg, TranSpot model)
        {
            TblTravel tblTravel = new TblTravel();

            var val = Request.Form["mydata"].ToString();
            if (val != null)
            {
                tblTravel.SpotDeparture = model.spot;
                tblTravel.SpotDestination = model.spot1;
                tblTravel.Name = model.Name;
                tblTravel.Price = model.Price;
                tblTravel.Status = model.Active ? "active" : "deactive"; ;
                tblTravel.Description = val;
                tblTravel.TransCategoryId = model.Tran;
                _context.TblTravels.Add(tblTravel);
                int newid = _context.SaveChanges();
                if (newid > 0) // Check for successful save
                {
                    var newlyCreatedId = tblTravel.TravelId;
                    if (FileImg != null)
                    {
                        foreach (var file in FileImg)
                        {
                            string fileName = DateTime.Now.Ticks + file.FileName;
                            string uploadPath = Path.Combine(_env.WebRootPath, "img", "Travel");

                            string filePath = Path.Combine(uploadPath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            TblImageUrl tblImage = new TblImageUrl();
                            tblImage.Url = fileName;
                            tblImage.Description = "upload";
                            tblImage.ObjectId = newlyCreatedId;
                            tblImage.UrlObject = model.selectObject;

                            _context.TblImageUrls.Add(tblImage);
                            _context.SaveChanges();

                        }

                    }// Assuming Id property is auto-generated
                }

            }
            return Redirect("/admin/AdminTransportView");
        }

        public IActionResult ViewEditAdminTransport()
        {
            int id = int.Parse(Request.Query["idt"]);
            ITravelRepository travelRepository = new ITravelRepository(_context);
            GetCar_Plane_Train getCar_Plane_Train = new GetCar_Plane_Train();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            ImageRepository imageRepository = new ImageRepository(_context);
            string a = "Travel";
            var spot = spotRepository.GetAllSpot();
            var transpot = travelRepository.GetAllTransportation();
            var img = imageRepository.GetAllImg(id, a);
            var travel = travelRepository.GetTravelById(id);

            if (travel.Status == "active")
            {
                getCar_Plane_Train.Active = true;
            }
            else
            {
                getCar_Plane_Train.Active = false;
            }
            getCar_Plane_Train.Transportations = transpot;
            getCar_Plane_Train.Spots = spot;
            getCar_Plane_Train.travel = travel;
            getCar_Plane_Train.HrImages = img;
            /*ViewBag.Travel = travel;*/
            return View(getCar_Plane_Train);
        }

        public IActionResult EditAdminTransport(List<IFormFile> FileImg, TranSpot model)
        {
            ITravelRepository travelRepository = new ITravelRepository(_context);
            ImageRepository imageRepository = new ImageRepository(_context);
            TblTravel tblTravel = new TblTravel();
            var val = Request.Form["mydata"].ToString();
            int newlyCreatedId = model.id;
            /*var ls = imageRepository.GetAllId(a);
            string deletepath = Path.Combine(_env.WebRootPath, "img", "Travel");
            foreach (var item in ls)
            {
                string fileDelete = Path.Combine(deletepath, item.Url);
                System.IO.File.Delete(fileDelete);
            }*/


            if (val != null)
            {
                tblTravel.Name = model.Name;
                tblTravel.Description = val;
                tblTravel.SpotDeparture = model.spot;
                tblTravel.SpotDestination = model.spot1;
                tblTravel.Price = model.Price;
                tblTravel.Status = model.Active ? "active" : "deactive";
                tblTravel.ImageLinkId = null;
                tblTravel.TransCategoryId = model.Tran;
                travelRepository.EditTravel(newlyCreatedId, tblTravel);

            }
            if (FileImg != null && model != null)
            {

                foreach (var file in FileImg)
                {
                    string fileName = DateTime.Now.Ticks + file.FileName;
                    string uploadPath = Path.Combine(_env.WebRootPath, "img", "Travel");

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    TblImageUrl tblImage = new TblImageUrl();
                    tblImage.Url = fileName;
                    tblImage.Description = "upload";
                    tblImage.ObjectId = newlyCreatedId;
                    tblImage.UrlObject = "Travel";
                    _context.TblImageUrls.Add(tblImage);
                    _context.SaveChanges();


                }

            }// Assuming Id property is 


            return Redirect("/admin/AdminTransportView");
        }

        [HttpPost]
        public IActionResult DeleteTranSportImg(int imageId)
        {
            ImageRepository imageRepository = new ImageRepository(_context);
            // Check if image ID is valid
            if (imageId <= 0)
            {
                return BadRequest("Invalid image ID."); // Return bad request if invalid ID
            }

            // Fetch image details from repository
            var image = imageRepository.GetById(imageId);

            // Check if image exists
            if (image == null)
            {
                return NotFound("Image not found."); // Return not found if image doesn't exist
            }

            // Build deletion path
            string deletePath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant", image.Url);

            try
            {
                // Attempt to delete file
                System.IO.File.Delete(deletePath);

                // Delete image record from database
                imageRepository.DeleteImg(imageId);

                // Return success response (optional)
                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(ex.Message);
            }
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
        public IActionResult AdminHotelView(int page = 1, int pageSize = 8)
        {
            IHotelRepository hotelRepository = new IHotelRepository(_context);


            string a = "Hotel_Restaurant";
            var all = hotelRepository.GetAllHotel_Res_Re(a, page, pageSize);





            return View(all);

        }
        public IActionResult ViewCreateAdminHotel(int getid)
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
            getHotel.selectObject = new SelectList(new List<string>
                {
                    "Hotel_Restaurant",
                    "Travel",
                    "Tourist_Place",
                    "Tour_Package"
                    });
            return View(getHotel);

        }
        public IActionResult CreateAdminHotel(HotelRestaurant model, List<IFormFile> FileImg, string editor)
        {
            TblHotelRestaurant tblHotelRestaurant = new TblHotelRestaurant();

            var val = Request.Form["mydata"].ToString();
            if (val != null)
            {
                tblHotelRestaurant.Name = model.Name;
                tblHotelRestaurant.Price = model.Price;
                tblHotelRestaurant.SpotId = model.spot;
                tblHotelRestaurant.Status = model.Active ? "active" : "deactive";
                tblHotelRestaurant.Description = val;
                tblHotelRestaurant.CatId = model.catid;

                _context.TblHotelRestaurants.Add(tblHotelRestaurant);
                int newid = _context.SaveChanges();

                if (newid > 0) // Check for successful save
                {
                    var newlyCreatedId = tblHotelRestaurant.HrId;
                    if (FileImg != null)
                    {
                        foreach (var file in FileImg)
                        {
                            string fileName = DateTime.Now.Ticks + file.FileName;
                            string uploadPath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant");

                            string filePath = Path.Combine(uploadPath, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            TblImageUrl tblImage = new TblImageUrl();
                            tblImage.Url = fileName;
                            tblImage.Description = "upload";
                            tblImage.ObjectId = newlyCreatedId;
                            tblImage.UrlObject = model.selectObject;
                            _context.TblImageUrls.Add(tblImage);
                            _context.SaveChanges();

                        }

                    }// Assuming Id property is auto-generated
                }

            }
            return Redirect("/admin/AdminHotelView");
        }



        public IActionResult ViewEditAdminHotel()
        {
            int q = int.Parse(Request.Query[("idt")]);
            string a = "Hotel_Restaurant";
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            GetHotel_Res_Re getHotel_Res_Re = new GetHotel_Res_Re();
            ISpotRepository spotRepository = new ISpotRepository(_context);
            GetHotel_Res_Re getHotel = new GetHotel_Res_Re();
            ImageRepository imageRepository = new ImageRepository(_context);



            var editht = hotelRepository.GetTblHotelRestaurantById(q);
            var list = hotelRepository.GetAllHR();
            var spot1 = spotRepository.GetAllSpot();
            var img = imageRepository.GetAllImg(q, a);
            getHotel.tblSpots = spot1;
            getHotel.HrCategories = list;
            getHotel.tblHotel = editht;
            if (editht.Status == "active")
            {
                getHotel.Active = true;
            }
            else
            {
                getHotel.Active = false;
            }

            /*ViewBag.editht = editht;*/
            getHotel.HrImages = img;
            return View(getHotel);
        }

        public IActionResult EditAdminHotel(HotelRestaurant model, List<IFormFile> FileImg)
        {
            int newlyCreatedId = model.Hrid;
            IHotelRepository hotelRepository = new IHotelRepository(_context);
            TblHotelRestaurant tblHotelRestaurant = new TblHotelRestaurant();
            var val = Request.Form["mydata"].ToString();
            if (val != null)
            {
                tblHotelRestaurant.Name = model.Name;
                tblHotelRestaurant.Price = model.Price;
                tblHotelRestaurant.SpotId = model.spot;
                tblHotelRestaurant.Status = model.Active ? "active" : "deactive";
                tblHotelRestaurant.Description = val;
                tblHotelRestaurant.CatId = model.catid;
                /* tblHotelRestaurant.Imglink = fileName;
                 tblHotelRestaurant.ImageLinkId = 1;*/
                hotelRepository.EditHotelRestaurant(newlyCreatedId, tblHotelRestaurant);

            }

            string deletepath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant");
            /*string fileDelete = Path.Combine(deletepath, ls.Imglink);
            System.IO.File.Delete(fileDelete);*/
            if (FileImg != null && model != null)
            {

                var ls = hotelRepository.GetTblHotelRestaurantById(newlyCreatedId);
                foreach (var file in FileImg)
                {
                    string fileName = DateTime.Now.Ticks + file.FileName;
                    string uploadPath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant");

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    TblImageUrl tblImage = new TblImageUrl();
                    tblImage.Url = fileName;
                    tblImage.Description = "upload";
                    tblImage.ObjectId = newlyCreatedId;
                    tblImage.UrlObject = "Hotel_Restaurant";
                    _context.TblImageUrls.Add(tblImage);
                    _context.SaveChanges();


                }

            }// Assuming Id property is 


            return Redirect("/admin/AdminHotelview");

        }

        [HttpPost]
      
        public IActionResult DeleteHotelImg(int imageId)
        {
            ImageRepository imageRepository = new ImageRepository(_context);
            // Check if image ID is valid
            if (imageId <= 0)
            {
                return BadRequest("Invalid image ID."); // Return bad request if invalid ID
            }

            // Fetch image details from repository
            var image = imageRepository.GetById(imageId);

            // Check if image exists
            if (image == null)
            {
                return NotFound("Image not found."); // Return not found if image doesn't exist
            }

            // Build deletion path
            string deletePath = Path.Combine(_env.WebRootPath, "img", "Hotel_Restaurant", image.Url);

            try
            {
                // Attempt to delete file
                System.IO.File.Delete(deletePath);

                // Delete image record from database
                imageRepository.DeleteImg(imageId);

                // Return success response (optional)
                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode( ex.Message);
            }
        }

        private IActionResult StatusCode(string message)
        {
            throw new NotImplementedException();
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
        public async Task<IActionResult> AdminProfile()
        {
            


            return View();
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
       

        [HttpPost]
        public async Task<IActionResult> CreateNews(CreateNewsViewModel model, List<IFormFile> files, List<string> fileDescriptions)
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
                        _logger.LogError($"ModelState error for {entry.Key}: {entry.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()}");
                    }
                }
                return View(model); // Return the view with errors displayed
            }

            try
            {
                // Create a new news item and assign values from the model to it
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

                // Check and save uploaded files along with their descriptions
                if (files != null && files.Count > 0)
                {
                    var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "img", news.NewsObject);
                    if (!Directory.Exists(uploadsFolderPath))
                    {
                        Directory.CreateDirectory(uploadsFolderPath);
                    }

                    for (int i = 0; i < files.Count; i++)
                    {
                        var formFile = files[i];
                        var description = (i < fileDescriptions.Count) ? fileDescriptions[i] : "No Description Provided";

                        if (formFile.Length > 0)
                        {
                            // Get the file extension of the original file
                            var fileExtension = Path.GetExtension(formFile.FileName);

                            // Adjust to UTC+7
                            var utcPlus7Time = DateTime.UtcNow.AddHours(7);

                            // Generate a new file name using the adjusted time's ticks
                            var newFileName = $"{utcPlus7Time.Ticks}{fileExtension}";

                            // Combine the new file name with the target directory path
                            var filePath = Path.Combine(uploadsFolderPath, newFileName);

                            // Save the file to the specified folder
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }

                            // Add file info to the database with its associated description
                            var imageUrl = new TblImageUrl
                            {
                                // Store the relative path
                                Url = newFileName,
                                ObjectId = news.ObjectId,
                                UrlObject = news.NewsObject,
                                Description = description
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
        public async Task<IActionResult> EditNews(int id, CreateNewsViewModel model, List<IFormFile> files, List<string> fileDescriptions)
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

                        for (int i = 0; i < files.Count; i++)
                        {
                            var formFile = files[i];
                            var description = (i < fileDescriptions.Count) ? fileDescriptions[i] : "No Description Provided";

                            if (formFile.Length > 0)
                            {
                                var fileExtension = Path.GetExtension(formFile.FileName);
                                var utcPlus7Time = DateTime.UtcNow.AddHours(7);
                                var newFileName = $"{utcPlus7Time.Ticks}{fileExtension}";
                                var filePath = Path.Combine(uploadsFolderPath, newFileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await formFile.CopyToAsync(stream);
                                }

                                // Add new image URLs with descriptions to the database
                                var imageUrl = new TblImageUrl
                                {
                                    Url = newFileName,
                                    ObjectId = newsToUpdate.ObjectId,
                                    UrlObject = newsToUpdate.NewsObject,
                                    Description = description
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
        public async Task<IActionResult> GetExistingImages(string newsObject, int objectId)
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

        //--------------------Feedbacks administration------------------------------//

        // Method to toggle the status
        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var feedback = _context.TblFeedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            // Toggle between "Active" and "Not Active"
            feedback.Status = feedback.Status == "Active" ? "Not Active" : "Active";
            _context.SaveChanges();

            return RedirectToAction(nameof(GetAllFeedBacks));
        }

        // GET: Feedback/EditFeedback/5
        [HttpGet]
        public IActionResult EditFeedback(int id)
        {
            var feedback = _context.TblFeedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedback/EditFeedback/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditFeedback(int id, [Bind("FeedbackId,Status")] TblFeedback updatedFeedback)
        {
            var feedback = _context.TblFeedbacks.Find(id);

            if (feedback == null)
            {
                return NotFound();
            }

            feedback.Status = updatedFeedback.Status;

            try
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(GetAllFeedBacks));
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to update status. Try again later.");
                return View(updatedFeedback);
            }
        }

        // GET: Feedback/DeleteFeedback/5
        [HttpGet]
        public IActionResult DeleteFeedback(int id)
        {
            var feedback = _context.TblFeedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return View(feedback);
        }

        // POST: Feedback/DeleteFeedback/5
        [HttpPost, ActionName("DeleteFeedback")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFeedbackConfirmed(int id)
        {
            var feedback = _context.TblFeedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            _context.TblFeedbacks.Remove(feedback);
            _context.SaveChanges();

            return RedirectToAction(nameof(GetAllFeedBacks));
        }

        // Assume this method already exists
        public IActionResult GetAllFeedBacks()
        {
            var feedbacks = _context.TblFeedbacks.OrderByDescending(f => f.Date).ToList();

            //foreach (var feedback in feedbacks)
            //{
            //    switch (feedback.FeedbackObject)
            //    {
            //        case "Company":
            //            // Simulate fetching company name based on ObjectId
            //            feedback.ObjectName = "KerNal";
            //            break;
            //        case "Hotel_Restaurant":
            //            // Fetch the matching record from the TblHotelRestaurants table
            //            var hotelRestaurant = _context.TblHotelRestaurants
            //                 .FirstOrDefault(h => h.HrId == feedback.ObjectId);

            //            // Ensure that a result was found before accessing its properties
            //            feedback.ObjectName = hotelRestaurant?.Name ?? "Unknown Hotel/Restaurant";
            //            break;
            //        case "Travel":
            //            var travel = _context.TblTravels
            //                 .FirstOrDefault(h => h.TravelId == feedback.ObjectId);

            //            // Ensure that a result was found before accessing its properties
            //            feedback.ObjectName = travel?.Name ?? "Unknown travel";
            //            break;
            //        case "Tourist_Place":
            //            var touristplace = _context.TblTouristPlaces
            //                 .FirstOrDefault(h => h.Id == feedback.ObjectId);

            //            // Ensure that a result was found before accessing its properties
            //            feedback.ObjectName = touristplace?.Name ?? "Unknown touristplace";
            //            break;
            //        case "Tour_Package":
            //            var tourpackage = _context.TblTourPackages
            //                 .FirstOrDefault(h => h.PackageId == feedback.ObjectId);

            //            // Ensure that a result was found before accessing its properties
            //            feedback.ObjectName = tourpackage?.Name ?? "Unknown tourpackage";
            //            break;
            //        default:
            //            feedback.ObjectName = "Unknown";
            //            break;
            //    }
            //}
            GetObjectname(feedbacks);
            return View(feedbacks);
        }

        private void GetObjectname(List<TblFeedback> feedbacks)
        {
            foreach (var feedback in feedbacks)
            {
                switch (feedback.FeedbackObject)
                {
                    case "Company":
                        // Simulate fetching company name based on ObjectId
                        feedback.ObjectName = "KerNal";
                        break;
                    case "Hotel_Restaurant":
                        var hotelRestaurant = _context.TblHotelRestaurants
                            .FirstOrDefault(h => h.HrId == feedback.ObjectId);
                        feedback.ObjectName = hotelRestaurant?.Name ?? "Unknown Hotel/Restaurant";
                        break;
                    case "Travel":
                        var travel = _context.TblTravels
                            .FirstOrDefault(t => t.TravelId == feedback.ObjectId);
                        feedback.ObjectName = travel?.Name ?? "Unknown travel";
                        break;
                    case "Tourist_Place":
                        var touristplace = _context.TblTouristPlaces
                            .FirstOrDefault(tp => tp.Id == feedback.ObjectId);
                        feedback.ObjectName = touristplace?.Name ?? "Unknown touristplace";
                        break;
                    case "Tour_Package":
                        var tourpackage = _context.TblTourPackages
                            .FirstOrDefault(tp => tp.PackageId == feedback.ObjectId);
                        feedback.ObjectName = tourpackage?.Name ?? "Unknown tourpackage";
                        break;
                    default:
                        feedback.ObjectName = "Unknown";
                        break;
                }
            }
        }

        // filter feedbacks accoring to feedbackobjects//
        public IActionResult GetFilteredFeedbacks(string category)
        {
            // Materialize the query to a list immediately to avoid deferred execution issues
            var feedbacks = _context.TblFeedbacks.ToList();
            GetObjectname(feedbacks);
            // Apply filtering logic based on the category
            if (!string.IsNullOrEmpty(category) && category != "All")
            {
                feedbacks = feedbacks.Where(f => f.FeedbackObject == category).ToList();
                GetObjectname(feedbacks);
            }

            // Order by date
            var sortedFeedbacks = feedbacks.OrderByDescending(f => f.Date).ToList();

            return PartialView("_FeedbackTableRows", sortedFeedbacks);
        }


        //    return RedirectToAction("Success");
        //}
        // - KHANH 06/05/2024

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
            var spots = _context.TblSpots.ToList();
            ViewBag.Spots = spots;

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
                        
                        foreach (var file in files)
                        {
                            string path = _environment.WebRootPath;
                            var filename = DateTime.Now.Ticks + "_" + Path.GetFileName(file.FileName);

                            string savePath = Path.Combine(path, "img", "TblTourist_Place", filename);
                            using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }


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
                var spots = _context.TblSpots.ToList();
                ViewBag.Spots = spots;

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
        public IActionResult EditTouristPlace(TblTouristPlace model, List<IFormFile> files)
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
                            var filename = DateTime.Now.Ticks + "_" + Path.GetFileName(file.FileName);

                            string savePath = Path.Combine(path, "img", "TblTourist_Place", filename);
                            using (var stream = new FileStream(savePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }


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
                    return Redirect("/admin/AdminTouristPlace");
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
