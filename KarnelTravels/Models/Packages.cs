namespace KarnelTravels.Models
{
    public class Packages
    {
        
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal TotalPrice { get; set; }
        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public int spot {  get; set; }

    }
}
