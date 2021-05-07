using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using FlightSE.Models;

namespace FlightSE.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FlightSE.Models.SearchQuery> SearchQuery { get; set; }
        public DbSet<FlightSE.Models.Location> Location { get; set; }
    }
}
