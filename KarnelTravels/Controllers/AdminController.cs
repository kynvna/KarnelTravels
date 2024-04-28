using KarnelTravels.Models;
using KarnelTravels.Repository;
using Microsoft.AspNetCore.Mvc;

namespace KarnelTravels.Controllers
{
    public class AdminController : Controller
    {
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<AdminController> _logger;

        //Consolidate the constructors into one
        public AdminController(KarnelTravelsContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
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
    }
}
