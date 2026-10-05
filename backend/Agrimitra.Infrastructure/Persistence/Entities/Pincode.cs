using System;
using System.Collections.Generic;

namespace Agrimitra.Infrastructure.Persistence.Entities;

public partial class Pincode
{
    public string Pincode1 { get; set; } = null!;

    public Guid CountryId { get; set; }

    public Guid? StateId { get; set; }

    public Guid? DistrictId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual District? District { get; set; }

    public virtual State? State { get; set; }
}
