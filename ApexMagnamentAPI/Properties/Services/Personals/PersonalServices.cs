using ApexMagnamentAPI.Properties.DTOs;
using ApexMagnamentAPI.Properties.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ApexMagnamentAPI.Properties.Services.Personals
{
    public class PersonalServices : IPersonalServices
    {
        private readonly ApexMagnamentContext _db;
        private readonly IMapper _mapper;

        public PersonalServices(ApexMagnamentContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<int> DeletePersonal(int personalId)
        {
            var personal = await _db.Personals.FindAsync(personalId);
            if (personal == null)
                return -1;
            _db.Personals.Remove(personal);
            return await _db.SaveChangesAsync();
        }

        public async Task<PersonalResponse> GetPersonal(int personalId)
        {
            var personal = await _db.Personals.FindAsync(personalId);
            var personalResponse = _mapper.Map<Personal, PersonalResponse>(personal);

            return personalResponse;
        }

        public async Task<List<PersonalResponse>> GetPersonals()
        {
            var personal = await _db.Personals.ToListAsync();
            var personalList = _mapper.Map<List<Personal>, List<PersonalResponse>>(personal);

            return personalList;
        }

        public async Task<UserResponse> Login(UserRequest user)
        {
            var userEntity = await _db.Personals
                .FirstOrDefaultAsync(o => o.User == user.User);

            if (userEntity == null)
            {
                return null; // El usuario no existe
            }

            // Asegúrate de que los argumentos estén en este orden
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userEntity.Password))
            {
                return null; // La contraseña no coincide
            }

            // Si todo es correcto, el login es exitoso
            var userResponse = new UserResponse
            {
                UserId = userEntity.Id,
                User = userEntity.User,
                rolId = userEntity.RolId
            };

            return userResponse;
        }

        public async Task<int> PostPersonal(PersonalRequest personal)
        {
            // Verifica si ya existe un registro con los mismos datos
            var exists = await _db.Personals.AnyAsync(p =>
                p.Nombre == personal.Nombre &&
                p.Apellido == personal.Apellido &&
                p.Correo == personal.Correo &&
                p.Telefono == personal.Telefono &&
                p.User == personal.User);

            if (exists)
            {
                // Puedes lanzar una excepción, devolver 0 o un código de error
                throw new InvalidOperationException("Ya existe un registro de personal con los mismos datos.");
            }

            var personalRequest = _mapper.Map<PersonalRequest, Personal>(personal);

            // Hashea la contraseña usando BCrypt
            personalRequest.Password = BCrypt.Net.BCrypt.HashPassword(personalRequest.Password);

            await _db.Personals.AddAsync(personalRequest);
            await _db.SaveChangesAsync();
            return personalRequest.Id;
        }

        public async Task<int> PutPersonal(int personalId, PersonalRequest personal)
        {
            var entity = await _db.Personals.FindAsync(personalId);
            if (entity == null)
            {
                return -1;
            }

            var exists = await _db.Personals.AnyAsync(p => 
            p.Nombre == personal.Nombre &&
            p.Apellido == personal.Apellido &&
            p.Telefono == personal.Telefono &&
            p.Correo == personal.Correo &&
            p.User == personal.User);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un registro de personal con los mismos datos.");
            }

            // Almacena la contraseña actual antes del mapeo
            var currentPassword = entity.Password;
            _mapper.Map(personal, entity);

            // Verifica si la nueva contraseña no es nula o vacía
            if (!string.IsNullOrEmpty(personal.Password))
            {
                // Si hay una nueva contraseña, la hashea y actualiza la entidad
                entity.Password = BCrypt.Net.BCrypt.HashPassword(personal.Password);
            }
            else
            {
                // Si la contraseña es nula o vacía en la solicitud,
                // restablece la contraseña original para que no se borre
                entity.Password = currentPassword;
            }

            // Actualiza la entidad en la base de datos
            _db.Personals.Update(entity);

            return await _db.SaveChangesAsync();
        }

        // En la implementación de tu servicio, PersonalServices
        public async Task<PersonalResponse?> BuscarPersonal(string? nombre, string? apellido, string? telefono, string? correo)
        {
            var query = _db.Personals.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                query = query.Where(p => p.Nombre.ToLower().Contains(nombre.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(apellido))
            {
                query = query.Where(p => p.Apellido.ToLower().Contains(apellido.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(telefono))
            {
                query = query.Where(p => p.Telefono.ToLower().Contains(telefono.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(correo))
            {
                query = query.Where(p => p.Correo.Contains(correo.ToLower()));
            }

            var personal = await query.FirstOrDefaultAsync();

            if (personal == null)
            {
                return null;
            }

            var personalResponse = _mapper.Map<Personal, PersonalResponse>(personal);

            return personalResponse;
        }



    }
}
