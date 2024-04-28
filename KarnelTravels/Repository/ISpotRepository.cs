using KarnelTravels.Models;

namespace KarnelTravels.Repository
{
    public class ISpotRepository
    {
        private readonly KarnelTravelsContext _context;
        public ISpotRepository(KarnelTravelsContext context)
        {
            _context = context;
        }

        public TblSpot SearchSpot(string keyWord)
        {
            if (string.IsNullOrEmpty(keyWord))
            {
                return null; // Or throw an exception if empty keyword is invalid
            }

            var keywordLower = keyWord.ToLower();
            var q = _context.TblSpots
                .Where(s => s.Name.ToLower().Contains(keywordLower))
                .FirstOrDefault();
            return q;
        }
    }
}
