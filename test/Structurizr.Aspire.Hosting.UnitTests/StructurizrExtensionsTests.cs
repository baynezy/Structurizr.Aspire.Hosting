using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Structurizr.Aspire.Hosting.UnitTests;

public class StructurizrExtensionsTests
{
    private const int DefaultPort = 8080;
    private readonly Faker _faker = new();

    [Fact]
    public void AddStructurizr_WhenSettingName_ThenNameShouldBeSet()
    {
        Prop.ForAll(ValidResourceNamesArbitrary(),
                name =>
                {
                    // arrange
                    var builder = DistributedApplication.CreateBuilder();

                    // act
                    var resource = builder.AddStructurizr(name)
                        .Resource;

                    // assert
                    resource.Name.Should()
                        .Be(name);
                }
            )
            .QuickCheckThrowOnFailure();
    }

    [Fact]
    public void AddStructurizr_WithDefaults_ThenImageShouldBeSet()
    {
        // arrange
        const string name = "Structurizr";
        var builder = DistributedApplication.CreateBuilder();

        // act
        var resource = builder.AddStructurizr(name)
            .Resource;

        // assert
        resource.Annotations.Any(x => x is ContainerImageAnnotation)
            .Should()
            .BeTrue();

        var image = resource.Annotations.OfType<ContainerImageAnnotation>()
            .FirstOrDefault();

        image.Should()
            .NotBeNull();
        image.Image.Should()
            .Be("structurizr/lite");
        image.Tag.Should()
            .Be("latest");
    }

    [Fact]
    public void AddStructurizr_WithCustomImageTag_ThenImageTagShouldBeSet()
    {
        // arrange
        const string name = "Structurizr";
        var builder = DistributedApplication.CreateBuilder();
        var imageTag = GenerateImageTag();

        // act
        var resource = builder.AddStructurizr(name, config => config.ImageTag = imageTag)
            .Resource;

        // assert
        resource.Annotations.Any(x => x is ContainerImageAnnotation)
            .Should()
            .BeTrue();

        var image = resource.Annotations.OfType<ContainerImageAnnotation>()
            .FirstOrDefault();

        image.Should()
            .NotBeNull();
        image.Image.Should()
            .Be("structurizr/lite");
        image.Tag.Should()
            .Be(imageTag);
    }

    private string GenerateImageTag()
    {
        var imageTag = _faker.Random.Word();

        // remove and characters that are not alphanumeric or fullstops from imageTag
        imageTag = new string(imageTag.Where(c => char.IsLetterOrDigit(c) || c == '.')
            .ToArray());

        return imageTag;
    }

    [Fact]
    public void AddStructurizr_WithDefaults_ThenPortsShouldBeSet()
    {
        // arrange
        const string name = "Structurizr";
        var builder = DistributedApplication.CreateBuilder();

        // act
        var resource = builder.AddStructurizr(name)
            .Resource;

        // assert
        resource.Annotations.Any(x => x is EndpointAnnotation)
            .Should()
            .BeTrue();

        var endpoint = resource.Annotations.OfType<EndpointAnnotation>()
            .FirstOrDefault();

        endpoint.Should()
            .NotBeNull();
        endpoint.Port.Should()
            .Be(DefaultPort);
        endpoint.TargetPort.Should()
            .Be(DefaultPort);
    }

    [Fact]
    public void AddStructurizr_WithCustomPort_ThenPortShouldBeSet()
    {
        // arrange
        const string name = "Structurizr";
        var builder = DistributedApplication.CreateBuilder();
        var port = _faker.Random.Int(1024, 65535);

        // act
        var resource = builder.AddStructurizr(name, config => config.Port = port)
            .Resource;

        // assert
        resource.Annotations.Any(x => x is EndpointAnnotation)
            .Should()
            .BeTrue();

        var endpoint = resource.Annotations.OfType<EndpointAnnotation>()
            .FirstOrDefault();

        endpoint.Should()
            .NotBeNull();
        endpoint.Port.Should()
            .Be(port);
        endpoint.TargetPort.Should()
            .Be(DefaultPort);
    }

    [Fact]
    public void AddStructurizr_WithDefaults_ThenBindMountShouldBeSet()
    {
        // arrange
        const string name = "Structurizr";
        var builder = DistributedApplication.CreateBuilder();

        // act
        var resource = builder.AddStructurizr(name)
            .Resource;

        // assert
        resource.Annotations.Any(x => x is ContainerMountAnnotation)
            .Should()
            .BeTrue();

        var bindMount = resource.Annotations.OfType<ContainerMountAnnotation>()
            .FirstOrDefault();

        bindMount.Should()
            .NotBeNull();
        bindMount.Source.Should()
            .Be(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Structurizr")));
        bindMount.Target.Should()
            .Be("/usr/local/structurizr");
    }

    [Fact]
    public void AddStructurizr_WithCustomBindMount_ThenBindMountShouldBeSet()
    {
        // arrange
        const string name = "Structurizr";
        var builder = DistributedApplication.CreateBuilder();
        var bindMountPath = _faker.System.DirectoryPath();

        // act
        var resource = builder.AddStructurizr(name, config => config.WorkspaceFilesPath = bindMountPath)
            .Resource;

        // assert
        resource.Annotations.Any(x => x is ContainerMountAnnotation)
            .Should()
            .BeTrue();

        var bindMount = resource.Annotations.OfType<ContainerMountAnnotation>()
            .FirstOrDefault();

        bindMount.Should()
            .NotBeNull();
        bindMount.Source!.Replace("/", "\\")
            .Should()
            .EndWith(bindMountPath.Replace("/", "\\"));
        bindMount.Target.Should()
            .Be("/usr/local/structurizr");
    }

    private static Arbitrary<string> ValidResourceNamesArbitrary()
    {
        return (
            from name in ValidResourceNameGenerator()
            select name
        ).ToArbitrary();
    }

    private static Gen<string> ValidResourceNameGenerator()
    {
        return ArbMap.Default.GeneratorFor<string>()
            .Where(name => !string.IsNullOrWhiteSpace(name)
                           // max length is 64 characters
                           && name.Length <= 64
                           // resource names can only contain digits, letters, and hyphens
                           && name.All(c => char.IsDigit(c) || char.IsAsciiLetter(c) || c == '-')
                           // resource names cannot start or end with a hyphen
                           && char.IsAsciiLetter(name[0])
                           // resource names cannot end with a hyphen
                           && char.IsAsciiLetter(name[^1]));
    }
}