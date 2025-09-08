using ApexMagnamentAPI.Properties.DTOs;
using ApexMagnamentAPI.Properties.Services.Personals;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApexMagnamentAPI.Properties.EndPoints
{
    public static class PersonalEndPoints
    {
        public static void add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/personal").WithTags("Personals");

            group.MapGet("/", async (IPersonalServices personalService) =>
            {
                var personals = await personalService.GetPersonals();
                return Results.Ok(personals);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener personales",
                Description = "Lista de todos los personales",

            });

            group.MapGet("/{id}", async (int id, IPersonalServices personalService) =>
            {
                var personal = await personalService.GetPersonal(id);
                if (personal == null)
                    return Results.NotFound();
                return Results.Ok(personal);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener personal por ID",
                Description = "Obtiene un personal específico mediante su ID",
            }).RequireAuthorization();

            group.MapPost("/", async (PersonalRequest personal, IPersonalServices personalService) =>
            {
                if (personal == null)
                    return Results.BadRequest();

                var id = await personalService.PostPersonal(personal);

                return Results.Created($"/api/personal/{id}", personal);
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo personal",
                Description = "Crea un nuevo personal en el sistema",
            });

            group.MapPut("/{id}", async (int id, PersonalRequest personal, IPersonalServices personalService) =>
            {
                var result = await personalService.PutPersonal(id, personal);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar personal",
                Description = "Actualiza la información de un personal existente",
            });

            group.MapDelete("/{id}", async (int id, IPersonalServices personalService) =>
            {
                var result = await personalService.DeletePersonal(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NoContent();
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar personal",
                Description = "Elimina un personal existente mediante su ID",
            });

            group.MapPost("/login", async (UserRequest user, IPersonalServices personalService, IConfiguration config) =>
            {
                var login = await personalService.Login(user);
                if (login == null)
                    return Results.Unauthorized();
                else
                {
                    var JwtSetting = config.GetSection("JwtSetting");
                    var secretKey = JwtSetting.GetValue<string>("SecretKey");
                    var issuer = JwtSetting.GetValue<string>("Issuer");
                    var audience = JwtSetting.GetValue<string>("Audience");

                    var tokenHadler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(secretKey);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {

                        Subject = new ClaimsIdentity(new[]
                      {
                          new Claim(ClaimTypes.Name, login.User),


                      }),
                        Expires = DateTime.UtcNow.AddHours(8),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHadler.CreateToken(tokenDescriptor);

                    var jwt = tokenHadler.WriteToken(token);

                    return Results.Ok(jwt);

                }

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Login personal",
                Description = "Generar toke para autenticacion",
            });



        }
    }
}
