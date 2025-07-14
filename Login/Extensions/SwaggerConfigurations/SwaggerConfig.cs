using System.Reflection;

namespace Login.API.Extensions.SwaggerConfigurations;
/// <summary>
/// Classe responsável pela configuração do Swagger na aplicação.
/// </summary>
internal static class SwaggerConfig
{
    /// <summary>
    /// Adiciona e configura o Swagger no container de injeção de dependência.
    /// Inclui filtros personalizados e os comentários XML gerados pela aplicação.
    /// </summary>
    /// <param name="services">Coleção de serviços da aplicação.</param>
    /// <param name="configuration">Configurações da aplicação.</param>
    /// <returns>Retorna a coleção de serviços configurada.</returns>
    internal static IServiceCollection AddSwaggerConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            // Filtro para definir valores padrões nas operações
            options.OperationFilter<SwaggerDefaltValues>();

            // Filtro para remover o parâmetro api-version da documentação
            options.OperationFilter<ApiVersionFilter>();

            // Obter a referência do assembly principal da aplicação
            var executingAssembly = Assembly.GetExecutingAssembly();

            // Definir o caminho do arquivo XML com os comentários da aplicação
            var xmlFile = $"{executingAssembly.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // Incluir os comentários XML no Swagger
            options.IncludeXmlComments(xmlPath);

            // Incluir comentários XML dos projetos referenciados que começam com "Login.API"
            var referencedProjectsXmlDocPaths = executingAssembly.GetReferencedAssemblies()
                .Where(assembly => assembly.Name != null && assembly.Name.StartsWith("Login.API", StringComparison.InvariantCultureIgnoreCase))
                .Select(assembly => Path.Combine(AppContext.BaseDirectory, $"{assembly.Name}.xml"));

            foreach (var xmlDocPath in referencedProjectsXmlDocPaths)
            {
                if (File.Exists(xmlDocPath))
                    options.IncludeXmlComments(xmlDocPath);
            }
        });

        return services;
    }
}