﻿using System;
using FlightSE.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FlightSE.Areas.Identity.IdentityHostingStartup))]
namespace FlightSE.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<FlightSEContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("FlightSEContextConnection")));

           //     services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
           //         .AddEntityFrameworkStores<FlightSEContext>();
            });
        }
    }
}