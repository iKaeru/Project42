using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MemoryCardsAPI
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
//            services.AddDbContext<TodoContext>(options =>
//                options.UseSqlServer(Configuration.GetConnectionString("MvcMovieContext")));

//            services.AddDbContext<TodoContext>(opt =>
//                opt.UseInMemoryDatabase("TodoList"));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}