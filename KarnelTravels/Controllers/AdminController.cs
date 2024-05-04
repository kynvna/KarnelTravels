using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            return View("Sitemap");
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
                News = new TblNews(),
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
        public IActionResult CreateNews(CreateNewsViewModel model)
        {
            //remove validation of those list//
            ModelState.Remove("StatusList");
            ModelState.Remove("NewsObjectList");
            ModelState.Remove("ObjectNameList");
           
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (entry.Value.Errors.Count > 0)
                    {
                        // This will log the key of the model state entry and the error message
                        _logger.LogError($"ModelState error for {entry.Key}: {entry.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()}");
                    }
                }
                return View(model); // Return the view with errors displayed
            }
            //create new news and assign values form model to it & save to database//
            try
            {
                var news = new TblNews
                {
                    ObjectId = model.News.ObjectId,
                    Description = model.News.Description,
                    Status = model.News.Status,
                    NewsObject = model.News.NewsObject,
                    HotNews = model.News.HotNews,
                    Date = DateTime.Now, // Assigning the current date and time
                    NewsDetail = model.News.NewsDetail
                };

                _context.TblNews.Add(news); // Correct usage: adding to the DbSet
                _context.SaveChanges(); // Save changes in the database

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
                News = news,
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
        public async Task<IActionResult> EditNews(int id, CreateNewsViewModel model)
        {
            ModelState.Remove("StatusList");
            ModelState.Remove("NewsObjectList");
            ModelState.Remove("ObjectNameList");

            if (id != model.News.NewsId)
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

                    newsToUpdate.ObjectId = model.News.ObjectId;
                    newsToUpdate.Description = model.News.Description;
                    newsToUpdate.Status = model.News.Status;
                    newsToUpdate.NewsObject = model.News.NewsObject;
                    newsToUpdate.HotNews = model.News.HotNews;
                    newsToUpdate.Date = DateTime.Now; // changing time when updating news too.
                    newsToUpdate.NewsDetail = model.News.NewsDetail;

                    _context.Update(newsToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(model.News.NewsId))
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
            model.StatusList = new SelectList(new List<string> { "Active", "Deactive" }, model.News.Status);
            model.NewsObjectList = new SelectList(new List<string>
        {
            "Hotel_Restaurant",
            "Travel",
            "Tourist_Place",
            "Tour_Package"
        }, model.News.NewsObject);
            model.ObjectNameList = new SelectList(GetObjectNamesFor("Hotel_Restaurant"), "Id", "Name", model.News.ObjectId);

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


    }
}
