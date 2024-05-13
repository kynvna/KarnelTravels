using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarnelTravels.Models;

public partial class TblFeedback
{
    public int FeedbackId { get; set; }

    public string? FeedbackObject { get; set; }

    public int? ObjectId { get; set; }

    public string? CustomerId { get; set; }

    public string? Feedback { get; set; }

    public int? Rating { get; set; }

    public string? Status { get; set; }

    public DateTime? Date { get; set; }

    public int? IsRead { get; set; }

    [NotMapped]
    public string ObjectName { get; set; }
}
