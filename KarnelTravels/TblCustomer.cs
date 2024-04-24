using System;
using System.Collections.Generic;

namespace KarnelTravels;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<TblFeedback> TblFeedbacks { get; set; } = new List<TblFeedback>();
}
