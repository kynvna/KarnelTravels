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

        public IEnumerable<TblTravel> GetAllTran()
        {
            var tran = _context.TblTravels.ToList();
            return tran;
        }
        public IEnumerable<TblTravel> GetAllCar()
        {
            var car = _context.TblTravels.Where(c => c.TransCategoryId == 1).ToList();
            return car;
        }

        public IEnumerable<TblTravel> GetAllPlane()
        {
            var plane = _context.TblTravels.Where(c => c.TransCategoryId == 3).ToList();
            return plane;
        }

        

        public IEnumerable<TblTravel> GetAllTrain()
        {
            var train = _context.TblTravels.Where(c => c.TransCategoryId == 2).ToList();
            return train;
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
        public IEnumerable<TblTravel>SpotSearch(int tran, int spot , int spot1)
        {
            var ls = _context.TblTravels.Where(s => s.SpotDeparture == spot && s.SpotDestination == spot1 && s.TransCategoryId == tran).ToList();
            return ls;
        }
        public IEnumerable<TblTravel> SearchTravel( string keyWord)
        {
            var travel = _context.TblTravels.Where(t => t.Name.Contains(keyWord)).ToList();
            return travel;
        }
    }
}
