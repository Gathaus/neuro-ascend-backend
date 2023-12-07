using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Neuro.Infrastructure.ApiDocumentation;

public static class SwaggerExtension
{
    public static void AddNeuroSwagger(this IServiceCollection services)
    {

        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(type =>
            {
                if (type.IsNested)
                {
                    return $"{type.Namespace}.{type.DeclaringType.Name}.{type.Name}";
                }

                return $"{type.Namespace}.{type.Name}";
            });
            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Poi Api", Version = "v1"});

            // c.SwaggerDoc("experimental",
            //     new OpenApiInfo
            //     {
            //         Title = "Experimental Api",
            //         Description = "These endpoints are experimental please use versioned endpoints",
            //         Version = "experimental"
            //     });
            //
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Geçerli bir Token giriniz.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securitySchema);

            var securityRequirement = new OpenApiSecurityRequirement();
            securityRequirement.Add(securitySchema, new[] { "Bearer" });
            c.AddSecurityRequirement(securityRequirement);
            
            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                if (docName == "experimental" && apiDesc.RelativePath.Contains("experimental"))
                    return true;
                if (docName == "v1" && !apiDesc.RelativePath.Contains("experimental"))
                    return true;
                return false;
            });
        });
    }
}
