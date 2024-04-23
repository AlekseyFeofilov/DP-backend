using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using DP_backend.Swagger;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DP_backend.Configurations;

internal static class SwaggerConfiguration
{
    internal static void ConfigureSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            options.EnableAnnotations();
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.OperationFilter<DefaultResponseOperationFilter>();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.SchemaFilter<NullableSchemaFilter>();

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            foreach (var fi in dir.EnumerateFiles("*.xml"))
            {
                var doc = XDocument.Load(fi.FullName);
                options.IncludeXmlComments(() => new XPathDocument(doc.CreateReader()), true);
            }
        });
    }
}

public class NullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var nullabilityInfoContext = new NullabilityInfoContext();

        if (context.ParameterInfo is not null)
        {
            var parameterNullability = nullabilityInfoContext.Create(context.ParameterInfo);
            if (parameterNullability is { ReadState: NullabilityState.NotNull })
            {
                schema.Nullable = false;
                schema.Required = new HashSet<string>();
            }

            return;
        }

        foreach (var (propertyName, propertySchema) in schema.Properties)
        {
            var propertyInfo = context.Type.GetProperties().FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase));
            if (propertyInfo is null) return;
            var propertyNullability = nullabilityInfoContext.Create(propertyInfo);
            if (propertyNullability is { ReadState: NullabilityState.NotNull })
            {
                propertySchema.Nullable = false;
                schema.Required ??= new HashSet<string>();
                schema.Required.Add(propertyName);
            }
        }
    }
}