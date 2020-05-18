using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace RestuarantWebApi.Utils
{
    public class RequestHeader : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "HeaderAuthorization",
                Required = false, // set to false if this is optional
                Description = "Bearer {token}",
                In = ParameterLocation.Header
            });
        }
    }
}
