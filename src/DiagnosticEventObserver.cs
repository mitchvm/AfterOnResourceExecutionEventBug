namespace AfterOnResourceExecutionEventBug;

internal class DiagnosticEventObserver : IObserver<KeyValuePair<string, object>>
{
    private readonly List<string> _observedEvents = new List<string>();
    public IReadOnlyList<string> ObservedEvents => _observedEvents;

    public DiagnosticEventObserver()
    {
    }

    public void OnCompleted() { }
    public void OnError(Exception error) { }
    public void OnNext(KeyValuePair<string, object> value) => _observedEvents.Add(value.Key);
}
