using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace AfterOnResourceExecutionEventBug;

public class Tests : IClassFixture<MvcServerHostFixture>
{
    private readonly MvcServerHostFixture _hostFixture;

    public Tests(MvcServerHostFixture hostFixture)
    {
        _hostFixture = hostFixture;
        
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public async Task Test(string route, string[] expectedEvents)
    {
        var client = _hostFixture.GetTestClient();
        var observer = new DiagnosticEventObserver();
        using var sourceSubscriber = new DiagnosticSourceSubscriber(observer);
        sourceSubscriber.Subscribe();
        var response = await client.GetAsync(route);
        foreach(var expectedEvent in expectedEvents)
        {
            Assert.Contains(expectedEvent, observer.ObservedEvents);
        }
    }

    public static IEnumerable<object[]> TestCases => new[]
    {
        new object[] {
            "/shortcircuit/none",
            new[]
            {
                BeforeResourceFilterOnResourceExecutionEventData.EventName,
                BeforeActionFilterOnActionExecutionEventData.EventName,
                AfterActionFilterOnActionExecutionEventData.EventName,
                AfterResourceFilterOnResourceExecutionEventData.EventName
            }
        },
        new object[] {
            "/shortcircuit/action",
            new[]
            {
                BeforeResourceFilterOnResourceExecutionEventData.EventName,
                BeforeActionFilterOnActionExecutionEventData.EventName,
                AfterActionFilterOnActionExecutionEventData.EventName,
                AfterResourceFilterOnResourceExecutionEventData.EventName
            }
        },
        new object[] {
            "/shortcircuit/resource",
            new[]
            {
                BeforeResourceFilterOnResourceExecutionEventData.EventName,
                AfterResourceFilterOnResourceExecutionEventData.EventName
            }
        },
    };
}