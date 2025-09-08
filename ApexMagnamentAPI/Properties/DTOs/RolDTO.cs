namespace ApexMagnamentAPI.Properties.DTOs
{
    public class RolResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class RolRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
    }

}
