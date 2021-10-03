namespace ScoreTracker.Client;

public static class HostBuilderExtensions
{
    public static WebAssemblyHostBuilder AddGrpcService<T>(this WebAssemblyHostBuilder builder) where T : class
    {
        builder.Services.TryAddSingleton<NullableReturnTypeInterceptor>();
        builder.Services.AddCodeFirstGrpcClient<T>(opt =>
        {
            opt.Address = new Uri(builder.HostEnvironment.BaseAddress);
            opt.ChannelOptionsActions.Add(grpcChannelOptions =>
            {
                grpcChannelOptions.HttpHandler = new GrpcWebHandler(new HttpClientHandler());
            });
        }).AddInterceptor<NullableReturnTypeInterceptor>();

        return builder;
    }
}