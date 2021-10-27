using Commander.Models;
using CommandService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class SqlCommandRepo : ICommandRepo
    {
        private CommandContext _context;

        public SqlCommandRepo(CommandContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentException(nameof(cmd));
            }

            _context.Commands.Add(cmd);
        }

        public void CreateCommand(int platformId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Platforms.Add(plat);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentException(nameof(cmd));
            }

            _context.Commands.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateCommand(Command cmd)
        {
            //doe niks
        }

    }
}
