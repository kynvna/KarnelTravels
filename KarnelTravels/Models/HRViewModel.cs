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
        public int? ImageLinkId { get; set; }
        public string Imglink { get; set; }
        public string Status { get; set; }
        public virtual HrCategory? Cat { get; set; }

    }
}
