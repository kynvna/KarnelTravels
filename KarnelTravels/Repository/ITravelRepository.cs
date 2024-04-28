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
        public IEnumerable<TblTravel> SearchTravel( string keyWord)
        {
            var travel = _context.TblTravels.Where(t => t.Name.Contains(keyWord)).ToList();
            return travel;
        }
    }
}
