using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblFeedback
{
    public int FeedbackId { get; set; }

    public string? FeedbackObject { get; set; }

    public int? ObjectId { get; set; }

    public int? CustomerId { get; set; }

    public string? Feedback { get; set; }

    public int? NumberofStar { get; set; }

    public string? Status { get; set; }

    public virtual TblCustomer? Customer { get; set; }
}
