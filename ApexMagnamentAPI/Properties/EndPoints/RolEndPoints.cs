
using Microsoft.OpenApi.Models;
using ApexMagnamentAPI.Properties.DTOs;
using ApexMagnamentAPI.Properties.Services.Rols;
namespace ApexMagnamentAPI.Properties.EndPoints
{
    public static class RolEndPoints
    {
        public static void add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/rol").WithTags("Rols");

            group.MapGet("/", async (IRolServices rolServices) =>
            {
                var rols = await rolServices.GetRols();
                return Results.Ok(rols);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener roles",
                Description = "Lista de todos los roles",
            });

            group.MapGet("/{id}", async (int id, IRolServices rolServices) =>
            {
                var rol = await rolServices.GetRol(id);
                if (rol == null)
                    return Results.NotFound();
                return Results.Ok(rol);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener rol por ID",
                Description = "Obtiene un rol específico mediante su ID",
            });

            group.MapPost("/", async (RolRequest rol, IRolServices rolServices) =>
            {
                if (rol == null)
                    return Results.BadRequest();

                var id = await rolServices.PostRol(rol);

                return Results.Created($"/api/rol/{id}", rol);


            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Crear nuevo rol",
                Description = "Crea un nuevo rol en el sistema",
            });

            group.MapPut("/{id}", async (int id, RolRequest rol, IRolServices rolServices) =>
            {


                var result = await rolServices.PutRol(id, rol);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.Ok(result);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Actualizar rol",
                Description = "Actualiza la información de un rol existente mediante su ID",

            });

            group.MapDelete("/{id}", async (int id, IRolServices rolServices) =>
            {
                var result = await rolServices.DeleteRol(id);
                if (result == -1)
                    return Results.NotFound();
                else
                    return Results.NoContent();
            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Eliminar rol",
                Description = "Elimina un rol existente mediante su ID",
            });

            group.MapGet("/status", async () =>
            {
                return Results.Ok(new { status = "Api Corriendo" });

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Check API Status",
                Description = "Verifies that the API is running",
            });


        }
    }
}
