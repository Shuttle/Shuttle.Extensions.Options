del .\bin\Release\*.*
dotnet pack Shuttle.Extensions.Options.csproj
dotnet nuget push .\bin\Release\Shuttle.Extensions.Options.*.nupkg