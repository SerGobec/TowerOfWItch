using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerOfWitch.Models
{
    class PlayersDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<UserSymbolPair> UserSymbolPairs { get; set; } = null!;

        public PlayersDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TowerOfWitchPlayersDb;Trusted_Connection=True;");
        }
    }


}
