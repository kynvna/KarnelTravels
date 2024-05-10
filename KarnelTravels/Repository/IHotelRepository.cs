using KarnelTravels.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Security.AccessControl;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace KarnelTravels.Repository
{
    public class IHotelRepository
    {
        private readonly KarnelTravelsContext _context;
        public IHotelRepository (KarnelTravelsContext context)
        {
            _context = context;
        }
        public GetHotel_Res_Re GetAllHotel_Res_Re(string Ob,int page, int pageSize)
        {
           
            int totalItems = _context.TblHotelRestaurants.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var query = _context.TblHotelRestaurants.Skip((page - 1) * pageSize)
        .Take(pageSize).Select(a => new ViewHotelImg
        {
            HrId = a.HrId,
            Description = a.Description,
            SpotId = a.SpotId,
            Name = a.Name,
            Price = a.Price,
            CatId = a.CatId,
            url = (from b in _context.TblImageUrls where b.ObjectId == a.HrId && b.UrlObject == Ob select b.Url).FirstOrDefault()
        }).ToList();
            return new GetHotel_Res_Re
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                All = query // Use the paginated items collection
            };

        }

        public IEnumerable<ViewHotelImg> GetAllHotel_Res_Re_image(string Ob)
        {
            var ls =from a in _context.TblHotelRestaurants
                    select new ViewHotelImg { HrId = a.HrId, Description = a.Description,SpotId = a.SpotId,
                        Name = a.Name,Price = a.Price,CatId = a.CatId,
                        url = (from b in _context.TblImageUrls where b.ObjectId == a.HrId && b.UrlObject == Ob select b.Url).FirstOrDefault() }
                    ;
            return ls.ToList();
        }
        public GetHotel_Res_Re GetAllHotel(string Ob, int page, int pageSize) 
        {

            int totalItems = _context.TblHotelRestaurants.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var ls = from a in _context.TblHotelRestaurants.Where(t => t.CatId == 1)/*.Skip((page - 1) * pageSize)
        .Take(pageSize)*/
                     select new ViewHotelImg
                {
                    HrId = a.HrId,
                    Description = a.Description,
                    SpotId = a.SpotId,
                    Name = a.Name,
                    Price = a.Price,
                    CatId = a.CatId,
                    url = (from b in _context.TblImageUrls where b.ObjectId == a.HrId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                }
                    ;
             return new GetHotel_Res_Re
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                All = ls // Use the paginated items collection
            };
        }
        public GetHotel_Res_Re GetAllRestaurant(string Ob, int page, int pageSize)
        {
            int totalItems = _context.TblHotelRestaurants.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var ls = from a in _context.TblHotelRestaurants.Where(t => t.CatId == 3)/*.Skip((page - 1) * pageSize)
        .Take(pageSize)*/
                     select new ViewHotelImg
                     {
                         HrId = a.HrId,
                         Description = a.Description,
                         SpotId = a.SpotId,
                         Name = a.Name,
                         Price = a.Price,
                         CatId = a.CatId,
                         url = (from b in _context.TblImageUrls where b.ObjectId == a.HrId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                     }
                    ;
            return new GetHotel_Res_Re
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                All = ls // Use the paginated items collection
            };
        }
        public GetHotel_Res_Re GetAllResort(string Ob, int page, int pageSize)
        {
            int totalItems = _context.TblHotelRestaurants.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var ls = from a in _context.TblHotelRestaurants.Where(t => t.CatId == 2)/*.Skip((page - 1) * pageSize)
        .Take(pageSize)*/
                     select new ViewHotelImg
                     {
                         HrId = a.HrId,
                         Description = a.Description,
                         SpotId = a.SpotId,
                         Name = a.Name,
                         Price = a.Price,
                         CatId = a.CatId,
                         url = (from b in _context.TblImageUrls where b.ObjectId == a.HrId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                     }
                    ;
            return new GetHotel_Res_Re
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                All = ls // Use the paginated items collection
            };
        }

        public TblHotelRestaurant GetTblHotelRestaurantById(int id)
        {
            return _context.TblHotelRestaurants.FirstOrDefault(t => t.HrId == id);
        }

        public ViewHotelImg GetTblHotelImgById(int id, string Ob)
        {
            // Combine filtering and projection into a single query for efficiency
            var viewHotelImg = _context.TblHotelRestaurants
                .Where(t => t.HrId == id)
                .Select(a => new ViewHotelImg
                {
                    HrId = a.HrId,
                    Description = a.Description,
                    SpotId = a.SpotId,
                    Name = a.Name,
                    Price = a.Price,
                    Status = a.Status,
                    CatId = a.CatId,
                    url = _context.TblImageUrls
                        .Where(b => b.ObjectId == a.HrId && b.UrlObject == Ob)
                        .Select(b => b.Url)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            return viewHotelImg;
        }
        public void EditHotelRestaurant(int id , TblHotelRestaurant model)
        {
            if(id != null && model != null)
            {
                var ht = _context.TblHotelRestaurants.Find(id);
                if(ht != null)
                {
                    _context.Attach(ht);

                    ht.Price = model.Price;
                    ht.Name = model.Name;
                    ht.Status = model.Status;
                    ht.SpotId = model.SpotId;
                    ht.Imglink = model.Imglink;
                    ht.ImageLinkId = model.ImageLinkId;
                    ht.Description = model.Description;
                    ht.CatId = model.CatId;



                    _context.SaveChanges();
                }
            }
        }

        public void DeleteHotel(int id)
        {
            try
            {
                if (id != null)
                {
                    var hotel = _context.TblHotelRestaurants.Find(id);
                    if (hotel != null)
                    {
                        _context.Remove(hotel);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        public IEnumerable<HrCategory> GetAllHR()
        {
            var ls = _context.HrCategories.ToList();
            return ls;
        }

        public IEnumerable<ViewHotelImg> SearchHotel(string keyWord , string Ob)
        {
            
            
            var hotels = from a in _context.TblHotelRestaurants.Where(h => h.Name.Contains(keyWord) || h.Description.Contains(keyWord) || h.Status.Contains(keyWord))
                select new ViewHotelImg
                {
                    HrId = a.HrId,
                    Description = a.Description,
                    SpotId = a.SpotId,
                    Name = a.Name,
                    Price = a.Price,
                    CatId = a.CatId,
                    url = (from b in _context.TblImageUrls where b.ObjectId == a.HrId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                }
                    ;
            return hotels.ToList();
            
        }
        public List<TblImageUrl> GetAllHotelImgById(int id, string Ob)
        {
            var ls = _context.TblImageUrls.Where(i => i.ObjectId == id && i.UrlObject == Ob).ToList();
            return ls;
        }
        public IEnumerable<TblHotelRestaurant> SearchHotelSpot(int id)
        {
            var hotels = _context.TblHotelRestaurants.Where(h => h.SpotId == id).ToList();
            return hotels;
        }
    }
}
