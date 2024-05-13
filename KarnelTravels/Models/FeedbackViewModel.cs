namespace KarnelTravels.Models
{
    public class FeedbackViewModel
    {
        public string CustomerEmail { get; set; }
        public string CommentText { get; set; }
        public int Rating { get; set; }
        public string FeedbackObject { get; set; }  
        public int ObjectId { get; set; }
        public string Status { get; set; }
        public int IsRead { get; set; }
        public bool IsSubmitted { get; set; } // Flag to indicate if the form has been submitted
    }
}
