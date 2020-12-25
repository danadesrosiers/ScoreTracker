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
                .AddGrpcService<IUserService>()
                .AddGrpcService<IResultService>()
                .AddGrpcService<IMeetService>()
                .AddGrpcService<IAthleteService>()
                .AddGrpcService<IClubService>();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AthleteService>();
            builder.Services.AddScoped<ClubService>();
            builder.Services.AddScoped<MeetService>();
            builder.Services.AddTransient<IRankStrategy, BreakTiesRankStrategy>();

            builder.Services.AddSingleton<StateContainerFactory>();
            builder.Services.AddTransient(sp => sp.GetRequiredService<StateContainerFactory>().GetState());

            await builder.Build().RunAsync();
        }
    }
}
