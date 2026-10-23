using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BK.EventOps.Domain.Persistence.Entities;

[Keyless]
[Table("test")]
public partial class Test
{
    [Column("id")]
    public int? Id { get; set; }
}
