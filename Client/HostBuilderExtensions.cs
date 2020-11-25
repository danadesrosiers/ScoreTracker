using System;
using System.Net.Http;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using ScoreTracker.Shared;

namespace ScoreTracker.Client
{
    public static class HostBuilderExtensions
    {
        public static WebAssemblyHostBuilder AddGrpcService<T>(this WebAssemblyHostBuilder builder) where T : class
        {
            builder.Services.AddSingleton<NullableReturnTypeInterceptor>();
            builder.Services.AddCodeFirstGrpcClient<T>(opt =>
            {
                opt.Address = new Uri(builder.HostEnvironment.BaseAddress);
                opt.ChannelOptionsActions.Add(grpcChannelOptions =>
                {
                    grpcChannelOptions.HttpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
                });
            }).AddInterceptor<NullableReturnTypeInterceptor>();

            return builder;
        }
    }
}