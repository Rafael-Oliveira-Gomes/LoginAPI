using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Login.API.Extensions.SwaggerConfigurations;

/// <summary>
/// Filtro do Swagger responsável por remover o parâmetro "api-version" da documentação das operações da API.
/// </summary>
internal class ApiVersionFilter : IOperationFilter
{
    /// <summary>
    /// Aplica o filtro que remove o parâmetro "api-version" da operação, evitando que ele apareça na documentação do Swagger.
    /// </summary>
    /// <param name="operation">Representa a operação da API no Swagger.</param>
    /// <param name="context">Contexto da operação contendo metadados adicionais.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parametersToRemove = operation.Parameters.Where(x => x.Name == "api-version").ToList();

        foreach (var parameter in parametersToRemove)
            operation.Parameters.Remove(parameter);
    }
}
