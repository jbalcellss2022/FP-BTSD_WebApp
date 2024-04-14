using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace ODMWebAPI.Services
{
    public class SwaggerFilterSchemaSampleCode : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Extensions.Add(
                "x-codeSamples", new OpenApiObject
                {
                    ["lang"] = new OpenApiString("JavaScript"),
                    ["source"] = new OpenApiString("console.log('');")
                }
            );
        }
    }
}
