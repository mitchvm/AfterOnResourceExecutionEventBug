using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Diagnostics;

namespace AfterOnResourceExecutionEventBug;

internal class DiagnosticSourceSubscriber : IDisposable, IObserver<DiagnosticListener>
{
    private IDisposable _listenerSubscription;
    private IDisposable _listenerSourceSubscription;
    private readonly object _lock = new();
    private int _disposed;
    private readonly DiagnosticEventObserver _observer;

    public DiagnosticSourceSubscriber(DiagnosticEventObserver observer)
    {
        _observer = observer;
    }

    private static readonly string[] EventNames = new[]
    {
        BeforeActionFilterOnActionExecutionEventData.EventName,
        AfterActionFilterOnActionExecutionEventData.EventName,
        BeforeResourceFilterOnResourceExecutionEventData.EventName,
        AfterResourceFilterOnResourceExecutionEventData.EventName
    };

    public void Subscribe()
    {
        if (_listenerSourceSubscription == null)
        {
            _listenerSourceSubscription = DiagnosticListener.AllListeners.Subscribe(this);
        }
    }

    public void OnNext(DiagnosticListener listener)
    {
        if (listener.Name == "Microsoft.AspNetCore")
        {
            lock (_lock)
            {
                _listenerSubscription?.Dispose();
                _listenerSubscription = listener.Subscribe(_observer, eventName => EventNames.Contains(eventName));
            }
        }
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 1)
        {
            return;
        }

        lock (_lock)
        {
            _listenerSubscription?.Dispose();
            _listenerSubscription = null;
        }

        _listenerSourceSubscription?.Dispose();
        _listenerSourceSubscription = null;
    }
}
