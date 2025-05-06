using Microsoft.EntityFrameworkCore;
using OrdersApi;
using Repository.Coconseconsentext;
using Repository.Interface;
using Repository.Repositories;


public class Program {

    protected static void Main(string[] args) {

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });

}

