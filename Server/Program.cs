using ScoreTracker.Server;
using ScoreTracker.Server.Clients;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Server.MeetResultsProviders;
using ScoreTracker.Server.MeetResultsProviders.MyUsaGym;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

WebApplication
    .CreateBuilder(args)
    .ConfigureServices()
    .Build()
    .ConfigureRequestPipeline()
    .Run();

static class WebApplicationExtensions
{
    internal static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddCosmosClient(configuration.GetSection("CosmosDb"), c => c
            .AddCollection<User>()
            .AddCollection<Meet>()
            .AddCollection<MeetResult>("/meetId")
            .AddCollection<Club>()
            .AddCollection<Athlete>());

        services
            .AddTransient<IUserClient, UserClient>()
            .AddTransient<IAthleteClient, AthleteClient>()
            .AddTransient<IClubClient, ClubClient>()
            .AddTransient<IMeetClient, MeetClient>()
            .AddTransient<IMeetResultClient, MeetResultClient>()
            .AddSingleton<IMeetResultsProvider, MyUsaGymMeetResultsProvider>();

        services.AddHttpClient<MyUsaGymMeetResultsProvider>();

        services.AddGrpc(options => { options.Interceptors.Add<NullableReturnTypeInterceptor>(); });
        services.AddCodeFirstGrpc(config =>
        {
            config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
        });
        services.AddHostedService<MeetLoaderService>();
        services.AddHostedService<ResultNotificationService>();

        return builder;
    }

    internal static WebApplication ConfigureRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
        app.MapRazorPages();
        app.MapGrpcService<IMeetClient>();
        app.MapGrpcService<IMeetResultClient>();
        app.MapGrpcService<IUserClient>();
        app.MapGrpcService<IAthleteClient>();
        app.MapGrpcService<IClubClient>();
        app.MapFallbackToFile("index.html");

        return app;
    }
}