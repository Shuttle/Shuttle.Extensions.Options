namespace Shuttle.Extensions.Options;

public delegate Task AsyncEventHandler<in T>(T eventArgs);