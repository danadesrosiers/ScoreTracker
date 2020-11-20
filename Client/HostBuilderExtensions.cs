using System.Net.Http;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using ScoreTracker.Shared.Subscriptions;

namespace ScoreTracker.Client
{
    public static class HostBuilderExtensions
    {
        public static WebAssemblyHostBuilder AddGrpcService<T>(this WebAssemblyHostBuilder builder) where T : class
        {
            builder.Services.AddScoped(_ =>
            {
                var channel = GrpcChannel.ForAddress(builder.HostEnvironment.BaseAddress, new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()))
                });
                return channel.CreateGrpcService<T>();
            });

            return builder;
        }
    }
}