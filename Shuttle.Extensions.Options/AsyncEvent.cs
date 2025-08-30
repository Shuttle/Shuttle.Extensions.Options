using Shuttle.Core.Contract;

namespace Shuttle.Extensions.Options;

public class AsyncEvent<T> : IDisposable
{
    private readonly List<AsyncEventHandler<T>> _handlers = [];
    private readonly object _lock = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public static AsyncEvent<T> operator +(AsyncEvent<T> asyncEvent, AsyncEventHandler<T> asyncEventHandler)
    {
        Guard.AgainstNull(asyncEventHandler);

        lock (Guard.AgainstNull(asyncEvent)._lock)
        {
            if (!asyncEvent._handlers.Contains(asyncEventHandler))
            {
                asyncEvent._handlers.Add(asyncEventHandler);
            }
        }

        return asyncEvent;
    }

    public static AsyncEvent<T> operator -(AsyncEvent<T> asyncEvent, AsyncEventHandler<T> asyncEventHandler)
    {
        Guard.AgainstNull(asyncEventHandler);

        lock (Guard.AgainstNull(asyncEvent._lock))
        {
            asyncEvent._handlers.Remove(asyncEventHandler);
        }

        return asyncEvent;
    }

    public async Task InvokeAsync(T eventArgs)
    {
        List<AsyncEventHandler<T>> handlers;

        await _semaphore.WaitAsync();

        try
        {
            handlers = _handlers.ToList();
        }
        finally
        {
            _semaphore.Release();
        }

        if (handlers.Count == 0)
        {
            return;
        }

        await Task.WhenAll(handlers.Select(handler => handler.Invoke(eventArgs)));
    }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _handlers.Count;
            }
        }
    }

    public bool Contains(AsyncEventHandler<T> handler)
    {
        lock (_lock)
        {
            return _handlers.Contains(handler);
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _handlers.Clear();
        }
    }

    public void Dispose()
    {
        _semaphore.Dispose();
    }
}