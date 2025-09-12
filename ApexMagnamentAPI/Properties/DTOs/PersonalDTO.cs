using ApexMagnamentAPI.Properties.Models;

namespace ApexMagnamentAPI.Properties.DTOs
{
    public class PersonalResponse
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public byte[]? ImgPerfil { get; set; }

        public string Correo { get; set; } = null!;

        public string? User { get; set; }

        public byte Status { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaSession { get; set; }

        public int rolId { get; set; }


    }

    public class PersonalRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public string? Telefono { get; set; }

        public byte[]? ImgPerfil { get; set; }

        public string Correo { get; set; } = null!;

        public string? User { get; set; }

        public string? Password { get; set; }

        public byte Status { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaSession { get; set; }

        public int rolId { get; set; } 

    }
}
