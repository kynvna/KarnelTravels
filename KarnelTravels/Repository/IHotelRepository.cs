using KarnelTravels.Models;
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
