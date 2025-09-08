using System;
using System.Collections.Generic;

namespace ApexMagnamentAPI.Properties.Models;

public partial class Rol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Personal> Personals { get; set; } = new List<Personal>();
}
