using LezeckyDenik.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LezeckyDenik.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Record> Records { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MainPage> MainPages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<StatisticData> StatisticsData { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Training> Trainings { get; set; }


    }
}
