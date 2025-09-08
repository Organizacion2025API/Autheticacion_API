using System;
using System.Collections.Generic;

namespace ApexMagnamentAPI.Properties.Models;

public partial class Personal
{
    public int Id { get; set; }

    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public byte[]? ImgPerfil { get; set; }

    public string Correo { get; set; } = null!;

    public string User { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte Status { get; set; }

    public DateTime FechaIngreso { get; set; }

    public DateTime? FechaSession { get; set; }

    public virtual Rol Rol { get; set; } = null!;
}
