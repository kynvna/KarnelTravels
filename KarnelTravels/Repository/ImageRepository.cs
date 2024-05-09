using KarnelTravels.Models;

namespace KarnelTravels.Repository
{
    public class ImageRepository
    {
        private readonly KarnelTravelsContext _context;
        public ImageRepository(KarnelTravelsContext context)
        {
            _context = context;
        }
        public IEnumerable<TblImageUrl> GetAllId(int id)
        {
            var ls = _context.TblImageUrls.Where(x => x.ObjectId == id);
            return ls;
        }

        public IEnumerable<TblImageUrl> GetAllImg(int id , string Ob)
        {
            var ls = _context.TblImageUrls.Where (x => x.ObjectId == id && x.UrlObject == Ob);
            return ls.ToList();
        }

        public TblImageUrl GetById(int id)
        {
            return _context.TblImageUrls.FirstOrDefault(x => x.Id == id);
        }
        public void DeleteImg(int id)
        {
            try
            {
                if (id != null)
                {
                    var img = _context.TblImageUrls.Find(id);
                    if (img != null)
                    {
                        _context.Remove(img);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { }
        }
        public List<TblImageUrl> FindId(int id)
        {
            var ls = _context.TblImageUrls.Where(_x => _x.ObjectId == id);
            return ls.ToList();
        }

        public IEnumerable<TblImageUrl> GetAllRes(int id)
        {
            var ls = _context.TblImageUrls.Where(x => x.ObjectId == id);
            return ls;
        }
    }
}
