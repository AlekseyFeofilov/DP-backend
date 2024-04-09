using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Azure;
namespace DP_backend.Swagger
{
    public class DefaultResponseOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!operation.Responses.ContainsKey("200"))
            {
                try
                {
                    operation.Responses.Add("200", new OpenApiResponse
                    {
                        Description = "Success",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            ["application/json"] = new()
                            {
                                Schema = context.MethodInfo.ReturnType.GenericTypeArguments.FirstOrDefault()?.GenericTypeArguments.FirstOrDefault() is null ?
                                    null :
                                    context.SchemaGenerator.GenerateSchema(context.MethodInfo.ReturnType.GenericTypeArguments.FirstOrDefault()?.GenericTypeArguments.FirstOrDefault(), context.SchemaRepository)
                            }
                        }
                    });
                }
                catch (Exception e)
                {

                }

            }

            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", new OpenApiResponse
                {
                    Description = "InternalServerError",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(Response), context.SchemaRepository)
                        }
                    }
                });
            }
        }
    }
}
