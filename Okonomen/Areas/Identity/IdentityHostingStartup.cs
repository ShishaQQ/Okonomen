using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Okonomen.Data;

[assembly: HostingStartup(typeof(Okonomen.Areas.Identity.IdentityHostingStartup))]
namespace Okonomen.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(
                       context.Configuration.GetConnectionString("DefaultConnection")));
                services.AddDatabaseDeveloperPageExceptionFilter();

                services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("RequireAuthenticatedUser", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });
                    options.AddPolicy("RequireAdminUser", policy =>
                    {
                        policy.RequireRole("Admin");
                    });
                });
            });

        }
    }
}