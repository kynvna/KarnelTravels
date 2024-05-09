namespace KarnelTravels.Models
{
    public class ViewHotelUser
    {
       public IEnumerable<TblImageUrl> Images { get; set; }
        public ViewHotelImg Hotels { get; set; }
    }
}
