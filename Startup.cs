using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidData2020.Data;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;



namespace CovidData2020
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
            //add connection string
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            //add this
            EnsureDatabase.For.SqlDatabase(connectionString);

            //create and configure an instance of the DbUp upgrader
            var upgrader = DeployChanges.To
                .SqlDatabase(connectionString, null)//we told DBUp where DB is
                .WithScriptsEmbeddedInAssembly(System.Reflection.Assembly.GetExecutingAssembly()) //look for SQL scripts embedded in our project
                .LogToConsole()
                .WithTransaction() //do DB migration in a transaction
                .Build();
            //do a Db migration if there are any pending SQL
            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();//perform actual migration
            }

            //register IDataRepository to make it available for Dependency Injection
            //this below code means whenever IDataRepository is referenced in a contructor, then substitute an instance of
            //the DataRepository class
            // AddScoped: create one instance in a given HTTP request
            // AddSingleton: create one instance for the lifetime of whole app
            // AddTransient: create one instance each time it is requested
            services.AddScoped<IDataRepository, DataRepository>();

            services.AddCors(option =>
            option.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:3000") //front end app host origin
                    .AllowCredentials()));
            services.AddControllers();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseCors("CorsPolicy");
         

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
