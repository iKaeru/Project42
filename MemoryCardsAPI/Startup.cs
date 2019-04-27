using MemoryCardsAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryCardsAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {   
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<PostgreContext>()
                .BuildServiceProvider();

            services.AddMvc();

            // Inject an implementation of ISwaggerProvider 
            services.AddSwaggerGen(swagger =>
                {
                    swagger.SwaggerDoc("v1",
                        new Swashbuckle.AspNetCore.Swagger.Info {Title = "MemoryCardsAPI", Version = "v1"});
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "MemoryCardsAPI");
            });

            app.UseMvc();
        }
    }
}