using Commander.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander
{
    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base(opt)
        {

        }

        //dit mapt de model Command aan een database tabel Commands
        public DbSet<Command> Commands { get; set; }



    }
}
