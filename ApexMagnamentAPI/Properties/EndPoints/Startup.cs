namespace ApexMagnamentAPI.Properties.EndPoints
{
    public static class Startup
    {
        public static void UseEndPoints(this WebApplication app)
        {
           RolEndPoints.add(app);
           PersonalEndPoints.add(app);
        }
    }
}
