using Microsoft.EntityFrameworkCore;
using ChatModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ChatAppBW.Authentication;
using ChatModels.Models;

namespace ChatAppBW.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("ChatDB"));
        }

        public DbSet<AvailableUsers> AvailableUsers { get; set; }

        public DbSet<IndividualChat> IndividualChats { get; set; }
    }
}
