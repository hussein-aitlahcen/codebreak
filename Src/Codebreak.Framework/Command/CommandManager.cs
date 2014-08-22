using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Codebreak.Framework.Command
{
    public sealed class CommandManager<C> where C : CommandContext
    {
        private readonly IList<Command<C>> _commands = new List<Command<C>>();

        public CommandManager() { }

        public bool Execute(C context)
        {
            foreach (var command in _commands.Where(command => command.Aliases.Contains(context.TextCommandArgument.NextWord())))
            {
                if (command.Execute(context))
                    return true;
            }

            return false;
        }

        public void RegisterCommands()
        {
            RegisterCommands(Assembly.GetCallingAssembly());
        }

        public void RegisterCommands(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof (Command<C>)))
                {
                    var command = Activator.CreateInstance(type) as Command<C>;
                    if (command != null)
                    {
                        AddCommand(command);
                    }
                }
            }
        }

        public void AddCommand(Command<C> command)
        {
            foreach (var alias in from c in _commands 
                                  from alias in command.Aliases 
                                  where c.Aliases.Contains(alias) 
                                  select alias)
            {
                throw new Exception(String.Format("CommandManager contains already a command with the alias `{0}`", alias));
            }
            command.RegisterNestedSubCommands();
            _commands.Add(command);
        }

        public void RemoveCommand(Command<C> command)
        {
            if(command == null) throw new ArgumentException("command");

            _commands.Remove(command);
        }
    }
}
