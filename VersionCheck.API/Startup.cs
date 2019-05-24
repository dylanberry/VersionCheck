using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VersionCheck.API.Config;
using VersionCheck.API.VersionCheck;
using VersionCheck.Common;

namespace VersionCheck.API
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
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ApiVersionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddScoped<IVersionCheckService, VersionCheckService>();
            services.AddScoped<MinimumClientVersionFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(
                options => {
                    options.Run(HandleExceptions);
                }
            );
            
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private async Task HandleExceptions(HttpContext context)
        {
            var ex = context.Features.Get<IExceptionHandlerFeature>();
            if (ex?.Error is ClientVersionNotSupportedException versionEx)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(versionEx)).ConfigureAwait(false);
            }
        }
    }
}
