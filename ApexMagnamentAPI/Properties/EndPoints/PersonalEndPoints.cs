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

            }).RequireAuthorization();

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

            group.MapGet("/BuscarPersonal", async (string? nombre, string? apellido, string? telefono, string? correo, IPersonalServices personalService) =>
            {
                var personalResponse = await personalService.BuscarPersonal(nombre, apellido, telefono, correo);

                // Si personalResponse es null, no se encontró ningún registro.
                if (personalResponse == null)
                {
                    return Results.NotFound("No se encontró personal que coincida con la búsqueda.");
                }

                // Si se encontró un registro, devuélvelo.
                return Results.Ok(personalResponse);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Buscar un único personal por nombre, apellido, telefono, correo",
                Description = "Busca el primer registro de personal que coincida con los criterios de nombre y/o apellido.",
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
            }).RequireAuthorization();

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
            }).RequireAuthorization();

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
            }).RequireAuthorization();

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

                    var roleName = login.rolId switch
                    {
                        1 => "Administrador",
                        2 => "Tecnico",
                        3 => "Empleado",
                        _ => "usuario" // Por defecto, si el ID no se reconoce
                    };

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {

                        Subject = new ClaimsIdentity(new[]
                      {
                          new Claim(ClaimTypes.Name, login.User),
                          new Claim(ClaimTypes.Role, roleName),
                          // Opcional: También puedes agregar el RoleId si lo necesitas
                          new Claim("rolid", login.rolId.ToString())
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

            group.MapGet("/status", async () =>
            {
                return Results.Ok(new { status = "Api Corriendo" });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Check API Status",
                Description = "Verificar que la api esta corriendo",
            });

        }
    }
}
