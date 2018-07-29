using BLL.Models.Game;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Context
{
    public class StoryContext : DbContext
    {
        public StoryContext(DbContextOptions<StoryContext> options)
            : base(options)
        {
            
        }


        public DbSet<StoryPool> StoryPools { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<StoryProcedureState> ProcedureStates { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
