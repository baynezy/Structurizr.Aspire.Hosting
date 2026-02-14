# Structurizr.Aspire.Hosting

[![NuGet version](https://img.shields.io/nuget/v/Structurizr.Aspire.Hosting.svg)](https://www.nuget.org/packages/Structurizr.Aspire.Hosting/)

This is an Aspire extension that enables hosting of [Structurizr](https://structurizr.com/) workspaces in Aspire.

## Features

* Hosting Structurizr workspaces in Aspire.
* Use local workspace mounted in Structurizr Lite docker container in your dev environment.

## Usage

### Defaults

```csharp
using AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddStructurizr("Structurizr");

await builder.Build()
    .RunAsync();
```

### Customizing the Structurizr Lite Docker Container

```csharp
using AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddStructurizr("Structurizr", config =>
{
    config.ImageTag = "2025.05.28";
    config.Port = 8081;
    config.WorkspaceFilesPath = "/path/to/your/structurizr/workspace";
});

await builder.Build()
    .RunAsync();
```

## License

This project is licensed under [Apache License 2.0](http://www.apache.org/licenses/LICENSE-2.0).