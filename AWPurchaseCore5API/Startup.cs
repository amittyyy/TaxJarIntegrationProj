using AWPurchase.Business.Interfaces;
using AWPurchase.Business.Models.TaxJar;
using AWPurchase.Business.Services;
using AWPurchaseCore5API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWPurchaseCore5API
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

            //AT. Dependency Injection here
            services.AddTransient<ISalesTaxRateService, SalesTaxRateService>();

            services.AddControllers();

            services.AddSwaggerDocument();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AWPurchaseCore5API", Version = "v1" });
            //});

            TaxJarApiToken.taxJarApiToken = Configuration.GetSection("ApiOptions").GetSection("TaxJarApiToken").Value;

            // AT. AWLogger Setup
            AwLoggingConfiguration.ConfigureLoggingTargets(Configuration.GetSection("ApiOptions").GetSection("Environment").Value);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AWPurchaseCore5API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseOpenApiSpecification();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
