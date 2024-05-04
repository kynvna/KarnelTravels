using KarnelTravels.Models;

namespace KarnelTravels.Repository
{
    public class ITourPackageRepository
    {
        private readonly KarnelTravelsContext _context;
        public ITourPackageRepository(KarnelTravelsContext context)
        {
            _context = context;
        }

        public IEnumerable<TblTourPackage> GetAllPackage()
        {
            var pack = _context.TblTourPackages.ToList();
            return pack;
        }

        public IEnumerable<TblTourPackage> SearchTourPackages(string keyWord)
        {
            var tourPackage = _context.TblTourPackages.Where(p => p.Description.Contains(keyWord) || p.Name.Contains(keyWord)).ToList();
            return tourPackage;
        }

        public IEnumerable<TblTourPackage> SearchTourPackagesSpot(int id)
        {
            var tourPackage = _context.TblTourPackages.Where(p => p.SportId == id).ToList();
            return tourPackage;
        }

    }
}
