// ReSharper disable once CheckNamespace
namespace Aspire.Hosting;

public static class StructurizrExtensions
{
    public static IResourceBuilder<StructurizrResource> AddStructurizr(this IDistributedApplicationBuilder builder,
        [ResourceName] string name, Action<StructurizrConfig>? configure = null)
    {
        var config = new StructurizrConfig();
        configure?.Invoke(config);
        var resource = new StructurizrResource(name);
        return builder.AddResource(resource)
            .WithImage($"structurizr/lite:{config.ImageTag}")
            .WithEndpoint(port: config.Port, targetPort: 8080)
            .WithUrl($"http://localhost:{config.Port.ToString()}", "View Diagrams")
            .WithBindMount(config.WorkspaceFilesPath, "/usr/local/structurizr");
    }
}

public class StructurizrResource(string name) : ContainerResource(name);

public class StructurizrConfig
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    /// <summary>
    /// The tag of the Structurizr Lite Docker image to use.
    /// </summary>
    public string ImageTag { get; set; } = "latest";

    /// <summary>
    /// The path to the structurizr workspace files on the host machine.
    /// </summary>
    public string WorkspaceFilesPath { get; set; } =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Structurizr"));

    /// <summary>
    /// The port on which the Structurizr Lite server will expose on the host machine.
    /// </summary>
    public int Port { get; set; } = 8080;
    // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
}