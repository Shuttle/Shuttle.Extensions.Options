# Shuttle.Extensions.Options

This package provides extensions for configuring and validating `AsyncEvent` options in .NET applications.

## Usage

```csharp
public class OperationEventArgs(string name)
{
    public string Name { get; } = name;
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
                        Console.WriteLine(args.Name);

                        await Task.CompletedTask;
                    };
                })
            .AddSingleton<IApplicationService, ApplicationService>()
            .BuildServiceProvider();

        _applicationService = serviceProvider.GetRequiredService<IApplicationService>();

        await _applicationService.WorkAsync();
    }
}
```