﻿using KarnelTravels.Models;
using KarnelTravels.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
            if (val == null)
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
        public IActionResult EditAdminTourPackage(TourPackage model , IFormFile FileImg)
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
            return Redirect("/admin/ViewCreateTourPackage");
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
    }
}
