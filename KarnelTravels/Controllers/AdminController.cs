using KarnelTravels.Models;
using KarnelTravels.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace KarnelTravels.Controllers
{
    public class AdminController : Controller
    {
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _env;

        //Consolidate the constructors into one
        public AdminController(KarnelTravelsContext context, ILogger<AdminController> logger, IWebHostEnvironment env)
        {
            _context = context;
            _logger = logger;
            _env = env;

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
                    return Redirect("/admin/AdminProfile");
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
        public IActionResult AdminTransportView(int page = 1, int pageSize = 8)
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
    }
}
