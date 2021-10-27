using Commander.Models;
using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// nb: niet alle methods van Interface geimplementeerd!
namespace Commander.Data
{
    public class MockCommandRepo : ICommandRepo
    {
        public void CreateCommand(Command cmd)
        {
            throw new NotImplementedException();
        }

        public void CreateCommand(int platformId, Command command)
        {
            throw new NotImplementedException();
        }

        public void CreatePlatform(Platform plat)
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
                new Command{Id=0, HowTo="fiets naar huis", Line="Pak je fiets", PlatformId=1},
                new Command{Id=1, HowTo="fiets naar werk", Line="Pak je fiets", PlatformId=1},
                new Command{Id=2, HowTo="fiets naar sportschool", Line="Pak je fiets", PlatformId=2}
            };

            return commands;
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            throw new NotImplementedException();
        }

        public Command GetCommandById(int id)
        {
            return new Command { Id = 2, HowTo = "fiets naar sportschool", Line = "Pak je fiets", PlatformId = 1 };
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            throw new NotImplementedException();
        }

        public bool PlatformExists(int platformId)
        {
            throw new NotImplementedException();
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
