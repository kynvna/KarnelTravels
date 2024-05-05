using KarnelTravels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace KarnelTravels.Controllers
{
    public class AdminController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly KarnelTravelsContext _context;
        private readonly ILogger<AdminController> _logger;

        //Consolidate the constructors into one
        public AdminController(IWebHostEnvironment environment, KarnelTravelsContext context, ILogger<AdminController> logger)
        {
            _environment = environment;
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
