using KarnelTravels.Models;

namespace KarnelTravels.Repository
{
    public class ITouristPlaceRepository
    {
        private readonly KarnelTravelsContext _context;
        public ITouristPlaceRepository( KarnelTravelsContext context) 
        {
            _context = context;
        }

        public IEnumerable<TblTouristPlace> GetAllTour()
        {
            var ls = _context.TblTouristPlaces.ToList();
            return ls;
        }
        public IEnumerable<TblTouristPlace> SearchTouristPlace(string keyWord)
        {
            var touristPlace = _context.TblTouristPlaces.Where(t => t.Name.Contains(keyWord) || t.Description.Contains(keyWord) || t.Status.Contains(keyWord)) 
            .ToList();
            return touristPlace;
        }

        public IEnumerable<TblTouristPlace> SearchTouristPlaceSpot(int id )
        {
            var touristPlace = _context.TblTouristPlaces.Where(t => t.SportId == id
            )
                .ToList();
            return touristPlace;
        }
    }
}
