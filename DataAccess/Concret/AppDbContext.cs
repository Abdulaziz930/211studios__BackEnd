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
    }
}
