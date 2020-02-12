using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using healthcheck.HealthChecks;
using healthcheck.Configurations;

namespace healthcheck
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

            //NOTE: Can use direct Connection String from appsettings.
            //AddSqlServer(Configuration["ConnectionStrings:DefaultConnection"])

            var dbConfig = new DatabaseConfiguration();
            Configuration.Bind("Database", dbConfig);

            services.AddHealthChecks()
                .AddSqlServer(BuildConnectionString(dbConfig))
                .AddCheck<DatabaseHealthCheck>("Database Health Check")
                .AddCheck<IntegrationHealthCheck>("Integration Health Check");
        }

        public static string BuildConnectionString(DatabaseConfiguration dbConfig)
            => $"Server={dbConfig.Server};Database={dbConfig.Database};User Id={dbConfig.User};Password={dbConfig.Password};";


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
