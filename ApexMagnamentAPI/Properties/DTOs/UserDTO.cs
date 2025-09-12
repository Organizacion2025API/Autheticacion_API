namespace ApexMagnamentAPI.Properties.DTOs
{
    public class UserResponse
    {
        public int UserId { get; set; } 
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int rolId { get; set; }


    }

    public class UserRequest
    {
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


    }
}
