// ReSharper disable once CheckNamespace

namespace Aspire.Hosting;

public static class StructurizrExtensions
{
    public static IResourceBuilder<StructurizrResource> AddStructurizr(this IDistributedApplicationBuilder builder,
        [ResourceName] string name, Action<StructurizrConfig>? configure = null)
    {
        var config = new StructurizrConfig
        {
            ImageTag = "latest",
            WorkspaceFilesPath = Path.GetFullPath(
                Path.Combine(
                    AppContext.BaseDirectory, "..", "..", "..", "..", "Structurizr")),
            Port = 8080
        };

        configure?.Invoke(config);

        var resource = new StructurizrResource(name);

        return builder.AddResource(resource)
            .WithImage($"structurizr/lite:{config.ImageTag}")
            .WithEndpoint(port: config.Port, targetPort: 8080)
            .WithUrl($"http://localhost:{config.Port.ToString()}", "View Diagrams")
            .WithBindMount(config.WorkspaceFilesPath, "/usr/local/structurizr");
    }
}