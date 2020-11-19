using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Subscriptions;

namespace ScoreTracker.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddGrpcClient<Meets.MeetsClient>(o =>
            {
                o.Address = new Uri(builder.HostEnvironment.BaseAddress);
            }).ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()));

            /*builder.Services.AddGrpcClient<ISubscriptionService>(o =>
            {
                o.Address = new Uri(builder.HostEnvironment.BaseAddress);
            }).ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()));*/

            builder.Services.AddGrpcClient<Results.ResultsClient>(o =>
            {
                o.Address = new Uri(builder.HostEnvironment.BaseAddress);
            }).ConfigurePrimaryHttpMessageHandler(() => new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler()));

            builder.Services.AddScoped(_ =>
            {
                var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
                var channel = GrpcChannel.ForAddress(
                    "https://localhost:44365/",
                    new GrpcChannelOptions { HttpClient = new HttpClient(handler) }
                );
                return channel;
            });

            builder.Services.AddScoped(sp =>
            {
                var channel = sp.GetRequiredService<GrpcChannel>();
                var service = channel.CreateGrpcService<ISubscriptionService>();
                return service;
            });

            await builder.Build().RunAsync();
        }
    }
}
