using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
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

            await builder.Build().RunAsync();
        }
    }
}
