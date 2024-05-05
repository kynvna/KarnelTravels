using Microsoft.AspNetCore.Mvc;

namespace KarnelTravels.Models
{
    public partial class HRViewModel
    {
        public int HrId { get; set; }
        public int? CatId { get; set; }
        public string Name { get; set; }
        public string SpotName { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } // Changed from Imglink to ImageUrl
        public string Status { get; set; }
        public virtual HrCategory? Cat { get; set; }
    }

}

