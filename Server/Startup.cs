using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Grpc.Server;
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
                .AddTransient<IUserClient, UserClient>()
                .AddTransient<IAthleteClient, AthleteClient>()
                .AddTransient<IClubClient, ClubClient>()
                .AddTransient<IMeetClient, MeetClient>()
                .AddTransient<IMeetResultClient, MeetResultClient>()
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
                endpoints.MapGrpcService<IMeetClient>();
                endpoints.MapGrpcService<IMeetResultClient>();
                endpoints.MapGrpcService<IUserClient>();
                endpoints.MapGrpcService<IAthleteClient>();
                endpoints.MapGrpcService<IClubClient>();
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
