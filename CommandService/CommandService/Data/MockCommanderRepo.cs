using Commander.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class MockCommanderRepo : ICommanderRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>()
            {
                new Command{Id=0, HowTo="fiets naar huis", Line="Pak je fiets", Platform="Gazelle"},
                new Command{Id=1, HowTo="fiets naar werk", Line="Pak je fiets", Platform="Gazelle"},
                new Command{Id=2, HowTo="fiets naar sportschool", Line="Pak je fiets", Platform="Gazelle"}
            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command { Id = 2, HowTo = "fiets naar sportschool", Line = "Pak je fiets", Platform = "Gazelle" };
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }
    }
}
