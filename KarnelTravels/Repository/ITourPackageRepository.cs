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
        public TblTourPackage GetPackageById(int id)
        {
            return _context.TblTourPackages.FirstOrDefault(t => t.PackageId == id);
        }

        public void DeletePack(int id)
        {
            try
            {
                if (id != null)
                {
                    var pack = _context.TblTourPackages.Find(id);
                    if (pack != null)
                    {
                        _context.Remove(pack);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
        public void EditPackage( int id , TblTourPackage model)
        {
            if (id != null && model != null)
            {
                var ht = _context.TblTourPackages.Find(id);
                if (ht != null)
                {
                    _context.Attach(ht);

                    ht.TotalPrice = model.TotalPrice;
                    ht.Name = model.Name;
                   ht.StartDate = model.StartDate;
                    ht.EndDate = model.EndDate;
                    ht.Description = model.Description;
                    ht.SportId = model.SportId;
                    ht.ImageLinkId = model.ImageLinkId;

                    _context.SaveChanges();
                }

            }
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
