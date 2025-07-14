using Microsoft.OpenApi.Models;
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
    /// <param name="operation">Representa a operação da API no Swagger.</param>
    /// <param name="context">Contexto da operação contendo metadados adicionais.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        // Define se o endpoint está obsoleto (deprecated) ao verificar o atributo [Obsolete]
        operation.Deprecated |= context.MethodInfo.GetCustomAttributes(true).OfType<ObsoleteAttribute>().Any();

        if (operation.Parameters == null)
            return;

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions
                                            .First(p => p.Name == parameter.Name);

            // Adiciona descrição do parâmetro, caso exista
            parameter.Description ??= description.ModelMetadata?.Description;

            // Define o valor default do parâmetro, caso exista
            if (description.DefaultValue != null)
            {
                // Serializa o valor default em JSON
                var json = System.Text.Json.JsonSerializer.Serialize(
                    description.DefaultValue,
                    description.ModelMetadata?.ModelType ?? typeof(object)
                );

                parameter.Schema.Default ??= OpenApiAnyFactory.CreateFromJson(json);
            }

            // Define se o parâmetro é obrigatório
            parameter.Required |= description.IsRequired;
        }
    }
}

