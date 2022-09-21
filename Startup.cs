using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Json_Based_Localization.Web
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Json_Based_Localization.Web", Version = "v1" });
            });
            services.AddLocalization(option=>option.ResourcesPath= "Resources");
            services.AddSingleton<IStringLocalizerFactory, jsonstringLocalizationfactory>();
            services.Configure<RequestLocalizationOptions>(option =>
            {
                var supportCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("ar-EG"),
                    new CultureInfo("de-DE"),
                };
                option.DefaultRequestCulture = new RequestCulture(culture: supportCultures[0], uiCulture: supportCultures[0]);
                option.SupportedCultures=supportCultures;
                option.SupportedUICultures=supportCultures;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Json_Based_Localization.Web v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            var supportCultures = new[] { "en-US", "ar-EG", "de-DE" };
             

            var localizationOptions= new RequestLocalizationOptions().
                SetDefaultCulture(supportCultures[0]).AddSupportedCultures(supportCultures).
                AddSupportedUICultures(supportCultures);
            app.UseRequestLocalization(localizationOptions);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
