using ApexMagnamentAPI.Properties.DTOs;
using ApexMagnamentAPI.Properties.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexMagnamentAPI.Properties.Services.Rols
{
    public class RolServices : IRolServices
    {
        private readonly ApexMagnamentContext _db;
        private readonly IMapper _mapper;

        public RolServices(ApexMagnamentContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeleteRol(int rolId)
        {
            var rol = await _db.Rols.FindAsync(rolId);
            if (rol == null)
                return -1;
            _db.Rols.Remove(rol);
            return await _db.SaveChangesAsync();
        }

        public async Task<RolResponse> GetRol(int rolId)
        {
            var rol = await _db.Rols.FindAsync(rolId);
            var rolResponse = _mapper.Map<Rol, RolResponse>(rol);

            return rolResponse;

        }

        public async Task<List<RolResponse>> GetRols()
        {
            var rol = await _db.Rols.ToListAsync();
            var rolList = _mapper.Map<List<Rol>, List<RolResponse>>(rol);

            return rolList;

        }

        public async Task<int> PostRol(RolRequest rol)
        {
            var exist = await _db.Rols.AnyAsync(r => 
            r.Nombre == rol.Nombre);

            if (exist)
            {
                throw new InvalidOperationException("Ya existe un registro de rol con los mismos datos.");
            }

            var rolRequest = _mapper.Map<RolRequest, Rol>(rol);
            await _db.Rols.AddAsync(rolRequest);

            await _db.SaveChangesAsync();

            return rolRequest.Id;
        }

        public async Task<int> PutRol(int rolId, RolRequest rol)
        {
            var entity = await _db.Rols.FindAsync(rolId);
            if (entity == null)
                return -1;

            entity.Nombre = rol.Nombre;
            _db.Rols.Update(entity);

            return await _db.SaveChangesAsync();
        }

        public async Task<RolResponse?> BuscarRol(string? nombre)
        {
            var rolRegistro = _db.Rols.AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                rolRegistro = rolRegistro.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));
            }

            var rol = await rolRegistro.FirstOrDefaultAsync();

            if (rol == null)
            {
                return null;
            }

            var rolResponse = _mapper.Map<Rol, RolResponse>(rol);

            return rolResponse;

        } 

    }
}
