using Microsoft.AspNetCore.Mvc;

namespace KarnelTravels.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
