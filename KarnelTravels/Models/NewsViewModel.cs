
using static KarnelTravels.Controllers.HomeController;

namespace KarnelTravels.Models;

    public class NewsViewModel
    {
        public int TotalNews { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

    public List<TblNewsWithImageUrls> News { get; set; }
}

