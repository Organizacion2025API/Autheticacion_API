
using ApexMagnamentAPI.Properties.DTOs;
using ApexMagnamentAPI.Properties.Services.Rols;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
namespace ApexMagnamentAPI.Properties.EndPoints
{
    public static class RolEndPoints
    {
        public static void add(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/rol").WithTags("Rols");

            //EndPoint para obtener todos lo registros de rol
            group.MapGet("/", async (IRolServices rolServices) =>
            {
                var rols = await rolServices.GetRols();
                return Results.Ok(rols);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Obtener roles",
                Description = "Lista de todos los roles",

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


            //EndPoint para obtener un registro de por id de rol
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

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


            //EndPoint para obtener un registro de rol por nomre
            group.MapGet("/BuscarRol", async (string? nombre, IRolServices rolServices) =>
            {
                var rolResponse = await rolServices.BuscarRol(nombre);

                // Si rolResponse es null, no se encontró ningún registro.
                if (rolResponse == null)
                {
                    return Results.NotFound("No se encontró rol que coincida con la búsqueda.");
                }

                // Si se encontró un registro, devuélvelo.
                return Results.Ok(rolResponse);

            }).WithOpenApi(o => new OpenApiOperation(o)
            {
                Summary = "Buscar rol por nombre",
                Description = "Obtiene un rol específico mediante el nombre",

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


            //EndPoint para crear nuevo rol
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

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


            //EndPoint para actualizar un rol
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

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });


            //EndPoint para eliminar un registro de rol
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

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });



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
