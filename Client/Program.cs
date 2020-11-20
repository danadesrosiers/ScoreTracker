using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Subscriptions;

namespace ScoreTracker.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            /*builder.Services.AddGrpcClient<ISubscriptionService>(o =>
            {
                o.Address = new Uri(builder.HostEnvironment.BaseAddress);
            }).ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()));*/

            builder.AddGrpcService<ISubscriptionService>();
            builder.AddGrpcService<IResultService>();
            builder.AddGrpcService<IMeetService>();

            await builder.Build().RunAsync();
        }
    }
}
