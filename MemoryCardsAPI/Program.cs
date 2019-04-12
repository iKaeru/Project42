using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace MemoryCardsAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}