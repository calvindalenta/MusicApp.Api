using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MusicApp.Api.Repositories;

namespace MusicApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddLettuceEncrypt();*/
            services.AddControllers();
            services.AddSingleton<IRepository, AudioRepository>();
            services.AddSpaStaticFiles(config =>
            {
                config.RootPath = "client";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => AllowAllCors(builder));
            } else
            {
                app.UseCors();
                app.UseSpaStaticFiles();
                /*app.UseHsts();*/
            }

            /*app.UseHttpsRedirection();*/

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(config =>
            {
                /*config.Options.SourcePath = "/";*/

                if (env.IsDevelopment())
                {
                    config.UseProxyToSpaDevelopmentServer("http://127.0.0.1:3000/react");
                }
            });

        }

        // Allow any origin for development purposes
        private CorsPolicyBuilder AllowAllCors(CorsPolicyBuilder builder)
        {
            // AllowAnyOrigin and AllowCredentials can't be used together
            // https://stackoverflow.com/questions/53675850/how-to-fix-the-cors-protocol-does-not-allow-specifying-a-wildcard-any-origin
            builder
            .SetIsOriginAllowed(_ => true)
            //.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();

            return builder;
        }
    }

/*    public static class TracksExtension
    {
        public static IServiceCollection AddTracks(this IServiceCollection services, IConfiguration config)
        {
            string jsonPath = PathUtils.GetFilePathFromApplicationPath(
                 config.GetValue<string>("TracksFileName")
                 );
            var jsonText = File.ReadAllText(jsonPath);
            var tracks = JsonSerializer.Deserialize<List<Track>>(jsonText);
            services.AddSingleton(tracks);
            return services;
        }
    }*/
}
