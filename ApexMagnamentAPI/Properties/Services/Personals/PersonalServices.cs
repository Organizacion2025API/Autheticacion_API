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
            var userEntity = await _db.Personals.
                FirstOrDefaultAsync(
                o => o.User == user.User && 
                o.Password == user.Password
                );

            var userResponse = _mapper.Map<Personal, UserResponse>(userEntity);
            return userResponse;
        }

        public async Task<int> PostPersonal(PersonalRequest personal)
        {
            var personalRequest = _mapper.Map<PersonalRequest, Personal>(personal);
            await _db.Personals.AddAsync(personalRequest);
            await _db.SaveChangesAsync();
            return personalRequest.Id;
        }

        public async Task<int> PutPersonal(int personalId, PersonalRequest personal)
        {
            var entity = await _db.Personals.FindAsync(personalId);
            if (entity == null)
                return -1;

            _mapper.Map(personal, entity);
            _db.Personals.Update(entity);

            return await _db.SaveChangesAsync();
        }
    }
}
