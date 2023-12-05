namespace Neuro.Api.Configurations;

public static class CorsConfiguration
{
    public static void AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(m =>
        {
            m.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
    }
}