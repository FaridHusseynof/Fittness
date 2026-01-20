using Fitness.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Data
{
    public class FitnessDbContext : IdentityDbContext<AppUser>
    {
        public FitnessDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Trainer> trainers { get; set; }
    }
}
