using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AfterOnResourceExecutionEventBug;

public class MvcServerHostFixture : IDisposable
{
    private readonly IHost _host;

    public IHost Host => _host;
    public HttpClient GetTestClient() => _host.GetTestClient();
    public TestServer GetTestServer() => _host.GetTestServer();

    public MvcServerHostFixture()
    {
        _host = new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddControllers();
                        services.AddRouting();
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());
                    });
            })
        .Start();
    }

    public void Dispose()
    {
        _host.Dispose();
    }
}