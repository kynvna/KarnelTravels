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
        public GetAllPack GetAllPackImg(string Ob, int page, int pageSize)
        {

            int totalItems = _context.TblTourPackages.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var query = _context.TblTourPackages.Skip((page - 1) * pageSize)
        .Take(pageSize).Select(a => new ViewPackageImg
        {
            PackageId = a.PackageId,
            Description = a.Description,
            SportId = a.SportId,
            Name = a.Name,
            TotalPrice = a.TotalPrice,
            EndDate = a.EndDate,
            StartDate = a.StartDate,
            url = (from b in _context.TblImageUrls where b.ObjectId == a.PackageId && b.UrlObject == Ob select b.Url).FirstOrDefault()
        }).ToList();
            return new GetAllPack
            {
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                Packages = query // Use the paginated items collection
            };

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
        public ViewPackageImg GetTblPackImgById(int id, string Ob)
        {
            // Combine filtering and projection into a single query for efficiency
            var viewPackImg = _context.TblTourPackages
                .Where(t => t.PackageId == id)
                .Select(a => new ViewPackageImg
                {
                   PackageId = a.PackageId,
                   Description = a.Description,
                   Name = a.Name,
                   EndDate = a.EndDate,
                   StartDate = a.StartDate,
                   SportId = a.SportId,
                   TotalPrice = a.TotalPrice,
                    url = _context.TblImageUrls
                        .Where(b => b.ObjectId == a.PackageId && b.UrlObject == Ob)
                        .Select(b => b.Url)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            return viewPackImg;
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
        public IEnumerable<ViewPackageImg> SearchTourPackages(string keyWord , string Ob)
        {
            var tourPackage = from a in _context.TblTourPackages.Where(p => p.Description.Contains(keyWord) || p.Name.Contains(keyWord))
                              select new ViewPackageImg
                              {
                                  PackageId = a.PackageId,
                                  Description = a.Description,
                                  Name = a.Name,
                                  EndDate = a.EndDate,
                                  StartDate = a.StartDate,
                                  SportId = a.SportId,
                                  TotalPrice = a.TotalPrice,
                                  url = (from b in _context.TblImageUrls where b.ObjectId == a.PackageId && b.UrlObject == Ob select b.Url).FirstOrDefault()
                              };
            return tourPackage.ToList();
        }

        public IEnumerable<TblTourPackage> SearchTourPackagesSpot(int id)
        {
            var tourPackage = _context.TblTourPackages.Where(p => p.SportId == id).ToList();
            return tourPackage;
        }

    }
}
