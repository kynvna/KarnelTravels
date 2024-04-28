using KarnelTravels.Models;

namespace KarnelTravels.Repository
{
    public class AdminLogin
    {
        private readonly KarnelTravelsContext _context;
        public AdminLogin(KarnelTravelsContext context)
        {
            _context = context;
        }

        public IEnumerable<TblAdminuser> Users()
        {
            var ls = _context.TblAdminusers.ToList();
            return ls;
        }
    }
}
