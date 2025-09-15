using ApexMagnamentAPI.Properties.DTOs;

namespace ApexMagnamentAPI.Properties.Services.Rols
{
    public interface IRolServices
    {
        Task<int> PostRol(RolRequest rol);
        Task<List<RolResponse>> GetRols();
        Task<RolResponse> GetRol(int rolId);
        Task<int> PutRol(int rolId, RolRequest rol);
        Task<int> DeleteRol(int rolId);
        Task<RolResponse> BuscarRol(string nombre);
    }
}
