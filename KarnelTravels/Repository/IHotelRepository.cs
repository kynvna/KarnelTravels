using KarnelTravels.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
namespace KarnelTravels.Repository
{
    public class IHotelRepository
    {
        private readonly KarnelTravelsContext _context;
        public IHotelRepository (KarnelTravelsContext context)
        {
            _context = context;
        }
        public IEnumerable<TblHotelRestaurant> GetAllHotel_Res_Re()
        {
            var ls = _context.TblHotelRestaurants.ToList();
            return ls;
        }
        public IEnumerable<TblHotelRestaurant> GetAllHotel()
        {
            var ls = _context.TblHotelRestaurants.Where(t => t.CatId == 1).ToList();
            return ls;
        }
        public IEnumerable<TblHotelRestaurant> GetAllRestaurant()
        {
            var ls = _context.TblHotelRestaurants.Where(t => t.CatId == 3).ToList();
            return ls;
        }
        public IEnumerable<TblHotelRestaurant> GetAllResort()
        {
            var ls = _context.TblHotelRestaurants.Where(t => t.CatId == 2).ToList();
            return ls;
        }

        public TblHotelRestaurant GetTblHotelRestaurantById(int id)
        {
            return _context.TblHotelRestaurants.FirstOrDefault(t => t.HrId == id);
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
            catch (Exception ex) { }
        }
        public IEnumerable<HrCategory> GetAllHR()
        {
            var ls = _context.HrCategories.ToList();
            return ls;
        }

        public IEnumerable<TblHotelRestaurant> SearchHotel(string keyWord)
        {
            var hotels = _context.TblHotelRestaurants.Where(h => h.Name.Contains(keyWord) || h.Description.Contains(keyWord) || h.Status.Contains(keyWord)).ToList();
            return hotels;
        }

        public IEnumerable<TblHotelRestaurant> SearchHotelSpot(int id)
        {
            var hotels = _context.TblHotelRestaurants.Where(h => h.SpotId == id).ToList();
            return hotels;
        }
    }
}
