using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concret
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameDetail> GameDetails { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<GameCategory> GameCategories { get; set; }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<GameDetailPlatform> GameDetailPlatforms { get; set; }

        public DbSet<Studio> Studios { get; set; }

        public DbSet<StudioDetail> StudioDetails { get; set; }

        public DbSet<Bio> Bios { get; set; }

        public DbSet<Social> Socials { get; set; }

        public DbSet<Banner> Banners { get; set; }
    }
}
