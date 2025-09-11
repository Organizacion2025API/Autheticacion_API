using Microsoft.OpenApi.Models;

namespace ApexMagnamentAPI.Properties.EndPoints
{
    public static class Startup
    {
        public static void UseEndPoints(this WebApplication app)
        {
           RolEndPoints.add(app);
           PersonalEndPoints.add(app);

            app .MapGet("/status", async () =>
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
