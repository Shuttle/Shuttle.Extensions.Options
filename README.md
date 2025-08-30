# Shuttle.Extensions.Options

::: code-group
```sh [.NET CLI]
dotnet add package Shuttle.Extensions.Options
```
```ps [PowerShell]
PM> Install-Package Shuttle.Extensions.Options
```
```xml [PackageReference]
<PackageReference Include="Shuttle.Extensions.Options" Version="<version>" />
```
```sh [packet]
paket add Shuttle.Extensions.Options
```
```sh [Script & Interactive]
#r "nuget: Shuttle.Extensions.Options, <version>"
```
```sh [File-based Apps]
#:package Shuttle.Extensions.Options@<version>
```
:::

This package provides extensions for configuring and validating `AsyncEvent` options in .NET applications.

## Usage

```csharp
public class OperationEventArgs(string operation)
{
    public string Operation { get; } = operation;
}

public class ApplicationOptions
{
    public AsyncEvent<OperationEventArgs> Operation { get; set; } = new();

    // other options...
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()

            .Configure<ApplicationOptions>(options =>
            {
                options.Operation += async args => 
                {
                    Console.WriteLine(args.Operation);

                    await Task.CompletedTask;
                };
                    
                options.Operation += _ => Task.CompletedTask;
            })

            .AddSingleton<IApplicationService, ApplicationService>()
            .BuildServiceProvider();

        _applicationService = serviceProvider.GetRequiredService<IApplicationService>();

        await _applicationService.WorkAsync();
    }
}
```