using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ScoreTracker.Client.Services;
using ScoreTracker.Client.Services.RankStrategy;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder
                .AddGrpcService<IUserClient>()
                .AddGrpcService<IMeetResultClient>()
                .AddGrpcService<IMeetClient>()
                .AddGrpcService<IAthleteClient>()
                .AddGrpcService<IClubClient>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAthleteService, AthleteService>();
            builder.Services.AddScoped<IClubService, ClubService>();
            builder.Services.AddScoped<IMeetService, MeetService>();
            builder.Services.AddTransient<IRankStrategy, BreakTiesRankStrategy>();

            builder.Services.AddSingleton<StateContainerFactory>();
            builder.Services.AddTransient(sp => sp.GetRequiredService<StateContainerFactory>().GetState());

            await builder.Build().RunAsync();
        }
    }
}
