using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RandApp.Areas.Identity.IdentityHostingStartup))]
namespace RandApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}