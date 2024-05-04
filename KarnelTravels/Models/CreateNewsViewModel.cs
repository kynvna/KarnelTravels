using Microsoft.AspNetCore.Mvc.Rendering;
namespace KarnelTravels.Models
{
    public class CreateNewsViewModel
    {
        public TblNews News { get; set; }
        public SelectList NewsObjectList { get; set; }  // This will hold the dropdown options
        public SelectList StatusList { get; set; }
        public SelectList ObjectNameList { get; set; }
    }
}
