using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebShell.Models;

namespace WebShell.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<CommandModel> Commands { get; set; }

    }

}
