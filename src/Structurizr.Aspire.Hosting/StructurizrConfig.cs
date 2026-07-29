// ReSharper disable once CheckNamespace
namespace Aspire.Hosting;

public class StructurizrConfig
{
    /// <summary>
    /// The tag of the Structurizr Lite Docker image to use.
    /// </summary>
    public required string ImageTag { get; set; }

    /// <summary>
    /// The path to the structurizr workspace files on the host machine.
    /// </summary>
    public required string WorkspaceFilesPath { get; set; }

    /// <summary>
    /// The port on which the Structurizr Lite server will expose on the host machine.
    /// </summary>
    public required int Port { get; set; }
}