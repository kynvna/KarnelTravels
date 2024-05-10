using Microsoft.AspNetCore.Mvc.Rendering;

namespace KarnelTravels.Models
{
    public class HotelRestaurant
    {
        public int Hrid { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int spot { get; set; }
         public int catid { get; set; }
         /*public string Des {  get; set; }*/
         public bool Active { get; set; }

        public string selectObject { get; set; }
    }
}
