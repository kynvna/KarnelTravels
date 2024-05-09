using KarnelTravels.Models;

namespace KarnelTravels.Repository
{
    public class ITravelRepository
    {
        private readonly KarnelTravelsContext _context;

        public ITravelRepository(KarnelTravelsContext context)
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
                    url = (from b in _context.TblImageUrls where b.ObjectId == a.TravelId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                }).ToList();
            return new GetCar_Plane_Train
            {
                PageSize = pageSize,

                CurrentPage = page,
                TotalPages = TotalPages,
                All = query
            };
        }
        public IEnumerable<TblTravel> GetAllTran()
        {
            var tran = _context.TblTravels.ToList();
            return tran;
        }
        public IEnumerable<ViewTravelImg> GetAllCar(string Ob)
        {
            var ls = from a in _context.TblTravels.Where(t => t.TransCategoryId == 1)
                     select new ViewTravelImg
                     {
                         Name = a.Name,
                         ImageLinkId = a.ImageLinkId,
                         Description = a.Description,
                         Price = a.Price,
                         SpotDeparture = a.SpotDeparture,
                         SpotDestination = a.SpotDestination,
                         Status = a.Status,
                         TransCategoryId = a.TransCategoryId,
                         TravelId = a.TravelId,

                         url = (from b in _context.TblImageUrls where b.ObjectId == a.TravelId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                     };
            return ls;
            
        }

        public IEnumerable<ViewTravelImg> GetAllPlane(string Ob)
        {
            var ls = from a in _context.TblTravels.Where(t => t.TransCategoryId == 3)
                     select new ViewTravelImg
                     {
                         Name = a.Name,
                         ImageLinkId = a.ImageLinkId,
                         Description = a.Description,
                         Price = a.Price,
                         SpotDeparture = a.SpotDeparture,
                         SpotDestination = a.SpotDestination,
                         Status = a.Status,
                         TransCategoryId = a.TransCategoryId,
                         TravelId = a.TravelId,

                         url = (from b in _context.TblImageUrls where b.ObjectId == a.TravelId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                     };
            return ls;
        }

        

        public IEnumerable<ViewTravelImg> GetAllTrain(string Ob)
        {
            var ls = from a in _context.TblTravels.Where(t => t.TransCategoryId == 2)
                     select new ViewTravelImg
                     {
                         Name = a.Name,
                         ImageLinkId = a.ImageLinkId,
                         Description = a.Description,
                         Price = a.Price,
                         SpotDeparture = a.SpotDeparture,
                         SpotDestination = a.SpotDestination,
                         Status = a.Status,
                         TransCategoryId = a.TransCategoryId,
                         TravelId = a.TravelId,

                         url = (from b in _context.TblImageUrls where b.ObjectId == a.TravelId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                     };
            return ls;
        }

        public ViewTravelImg GetTblTravelImgById(int id, string Ob)
        {
            // Combine filtering and projection into a single query for efficiency
            var viewTravelImg = _context.TblTravels
                .Where(t => t.TravelId == id)
                .Select(a => new ViewTravelImg
                {
                    TravelId=a.TravelId,
                    Name = a.Name,
                    Description = a.Description,
                    Price = a.Price,
                    SpotDeparture = a.SpotDeparture,
                    SpotDestination = a.SpotDestination,
                    Status = a.Status,
                    TransCategoryId = a.TransCategoryId,
                    ImageLinkId=a.ImageLinkId,
                    
                    url = _context.TblImageUrls
                        .Where(b => b.ObjectId == a.TravelId && b.UrlObject == Ob)
                        .Select(b => b.Url)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            return viewTravelImg;
        }

        public IEnumerable<TblTransportation> GetAllTransportation()
        {
            var ls = _context.TblTransportations.ToList();
            return ls;
        }

        public TblTravel GetTravelById(int id)
        {
            return _context.TblTravels.FirstOrDefault(t => t.TravelId == id);
        }
        public void DeleteTravel( int id)
        {
            try
            {
                if(id != null)
                {
                    var travels = _context.TblTravels.Find(id);
                    if(travels != null)
                    {
                        _context.Remove(travels);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { }
        }

        public void EditTravel(int id, TblTravel model)
        {
            if (id != null && model != null)
            {
                var ht = _context.TblTravels.Find(id);
                if (ht != null)
                {
                    _context.Attach(ht);

                    ht.Price = model.Price;
                    ht.Name = model.Name;
                    ht.Status = model.Status;
                    ht.SpotDeparture = model.SpotDeparture;
                    ht.SpotDestination = model.SpotDestination;
                    ht.Description = model.Description;
                    ht.ImageLinkId = model.ImageLinkId;
                   
                    ht.TransCategoryId = model.TransCategoryId;


                    _context.SaveChanges();
                }
            }
        }
        public IEnumerable<ViewTravelImg> SpotSearch(string Ob,int tran, int spot , int spot1)
        {

            var ls = from a in _context.TblTravels.Where(s => s.SpotDeparture == spot && s.SpotDestination == spot1 && s.TransCategoryId == tran)
                        select new ViewTravelImg
            {
                Name = a.Name,
                ImageLinkId = a.ImageLinkId,
                Description = a.Description,
                Price = a.Price,
                SpotDeparture = a.SpotDeparture,
                SpotDestination = a.SpotDestination,
                Status = a.Status,
                TransCategoryId = a.TransCategoryId,
                TravelId = a.TravelId,

                url = (from b in _context.TblImageUrls where b.ObjectId == a.TravelId && b.UrlObject == Ob select b.Url).FirstOrDefault()
            };
            return ls;
        }
        public IEnumerable<TblTravel> SearchTravel( string keyWord)
        {
            var travel = _context.TblTravels.Where(t => t.Name.Contains(keyWord)).ToList();
            return travel;
        }
    }
}
