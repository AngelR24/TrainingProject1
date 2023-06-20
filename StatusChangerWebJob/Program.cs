using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main()
    {
        var builder = new HostBuilder();
        builder.UseEnvironment(EnvironmentName.Development);
        builder.ConfigureWebJobs(b =>
        {
            b.AddAzureStorageCoreServices();
            b.AddAzureStorageQueues();
            
        });
        builder.ConfigureLogging((context, b) =>
        {
            b.AddConsole();
        });
        var host = builder.Build();
        using (host)
        {
            await host.RunAsync();
        }
    }
}