del .\bin\Release\*.* /Q
dotnet pack Shuttle.Extensions.Options.csproj
dotnet nuget push .\bin\Release\Shuttle.Extensions.Options.*.nupkg