using System;
using System.Collections.Generic;

namespace KarnelTravels.Models;

public partial class TblAdminuser
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PassWord { get; set; } = null!;
}
