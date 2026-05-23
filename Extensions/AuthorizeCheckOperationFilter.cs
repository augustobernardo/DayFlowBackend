using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DayFlowAPI.Extensions;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Security ??= new List<OpenApiSecurityRequirement>();

        var jwtAuthSchemeReference = new OpenApiSecuritySchemeReference("Bearer");
        operation.Security.Add(
            new OpenApiSecurityRequirement { { jwtAuthSchemeReference, new List<string>() } }
        );
    }
}
