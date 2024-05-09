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

        public GetCar_Plane_Train GetAllTravleImg(string Ob, int page, int pageSize)
        {
            var totalItem = _context.TblTravels.Count();
            var TotalPages = (int)Math.Ceiling((double)(totalItem) / pageSize);
            var query = _context.TblTravels.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(a => new ViewTravelImg
                {
                    Description = a.Description,
                    Name = a.Name,
                    Price = a.Price,
                    SpotDeparture = a.SpotDeparture,
                    SpotDestination = a.SpotDestination,
                    Status = a.Status,
                    TransCategoryId = a.TransCategoryId,
                    TravelId = a.TravelId,
                    url = (from b in _context.TblImageUrls where b.ObjectId == a.ImageLinkId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                }).ToList();
            return new GetCar_Plane_Train
            {
                PageSize = pageSize,

                CurrentPage = page,
                TotalPages = TotalPages,
                All = query
            };
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
