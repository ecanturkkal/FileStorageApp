using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FileStorageApp.API.Extensions
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileUploadParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile));

            if (fileUploadParams.Any())
            {
                operation.Parameters.Clear();
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content =
                {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties =
                            {
                                ["File"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                },
                                ["FolderId"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "uuid",
                                    Nullable = true
                                }
                            },
                            Required = new HashSet<string> { "File" }
                        }
                    }
                }
                };
            }
        }
    }
}
