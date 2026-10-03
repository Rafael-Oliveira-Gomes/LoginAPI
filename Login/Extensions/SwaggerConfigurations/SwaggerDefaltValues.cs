using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Login.API.Extensions.SwaggerConfigurations;

/// <summary>
/// Classe responsável por aplicar configurações padrões nas operações da documentação Swagger.
/// </summary>
public class SwaggerDefaltValues : IOperationFilter
{
    /// <summary>
    /// Aplica as configurações padrões em cada operação da API exibida no Swagger.
    /// </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<ObsoleteAttribute>()
            .Any();

        if (operation.Parameters == null)
            return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions
                .FirstOrDefault(p => p.Name == parameter.Name);

            if (description == null)
                continue;

            parameter.Description ??= description.ModelMetadata?.Description;
        }
    }
}