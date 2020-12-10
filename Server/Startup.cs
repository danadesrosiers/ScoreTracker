using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Grpc.Server;
using ScoreTracker.Server.Cosmos;
using ScoreTracker.Server.Services.Athletes;
using ScoreTracker.Server.Services.Clubs;
using ScoreTracker.Server.Services.Meets;
using ScoreTracker.Server.Services.Results;
using ScoreTracker.Server.Services.Results.MyUsaGym;
using ScoreTracker.Server.Services.Users;
using ScoreTracker.Shared;
using ScoreTracker.Shared.Athletes;
using ScoreTracker.Shared.Clubs;
using ScoreTracker.Shared.Meets;
using ScoreTracker.Shared.Results;
using ScoreTracker.Shared.Users;

namespace ScoreTracker.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddCosmosClient(Configuration.GetSection("CosmosDb"), c => c
                .AddCollection<User>()
                .AddCollection<Meet>()
                .AddCollection<MeetResult>("/meetId")
                .AddCollection<Club>()
                .AddCollection<Athlete>());

            services
                .AddSingleton<IMeetService, MeetService>()
                .AddSingleton<IClubService, ClubService>()
                .AddSingleton<IAthleteService, AthleteService>()
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IResultService, MeetResultService>()
                .AddSingleton<IMeetResultsProvider, MyUsaGymMeetResultsProvider>();

            services.AddHttpClient<MyUsaGymMeetResultsProvider>();

            services.AddGrpc(options =>
            {
                options.Interceptors.Add<NullableReturnTypeInterceptor>();
            });
            services.AddCodeFirstGrpc(config =>
            {
                config.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
            });
            services.AddHostedService<MeetLoaderService>();
            services.AddHostedService<ResultNotificationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<IMeetService>();
                endpoints.MapGrpcService<IResultService>();
                endpoints.MapGrpcService<IUserService>();
                endpoints.MapGrpcService<IAthleteService>();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
