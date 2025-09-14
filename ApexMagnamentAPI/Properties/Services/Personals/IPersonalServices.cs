using ApexMagnamentAPI.Properties.DTOs;

namespace ApexMagnamentAPI.Properties.Services.Personals
{
    public interface IPersonalServices
    {
        Task<int> PostPersonal(PersonalRequest personal);
        Task<List<PersonalResponse>> GetPersonals();
        Task<PersonalResponse> GetPersonal(int personalId);
        Task<int> PutPersonal(int personalId, PersonalRequest personal);
        Task<int> DeletePersonal(int personalId);
        Task<UserResponse> Login(UserRequest user);
        Task<PersonalResponse> BuscarPersonal(string? nombre, string? apellido, string? telefono, string? correo);
    }
}
