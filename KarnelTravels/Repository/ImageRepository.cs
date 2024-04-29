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

        public IEnumerable<TblImageUrl> GetAllRes(int id)
        {
            var ls = _context.TblImageUrls.Where(x => x.ObjectId == id);
            return ls;
        }
    }
}
