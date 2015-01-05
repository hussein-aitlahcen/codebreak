using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Codebreak.Framework.Command
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public sealed class CommandManager<C> where C : CommandContext
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IList<Command<C>> m_commands = new List<Command<C>>();

        /// <summary>
        /// 
        /// </summary>
        public IList<Command<C>> Commands
        {
            get
            {
                return m_commands;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CommandManager() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Execute(C context)
        {
            var word = context.TextCommandArgument.NextWord();
            foreach (var command in m_commands.Where(command => command.Aliases.Contains(word)))
            {
                if (command.Execute(context))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RegisterCommands()
        {
            RegisterCommands(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(Command<C> command)
        {
            foreach (var alias in from c in m_commands 
                                  from alias in command.Aliases 
                                  where c.Aliases.Contains(alias) 
                                  select alias)
            {
                throw new Exception(String.Format("CommandManager contains already a command with the alias `{0}`", alias));
            }
            command.RegisterNestedSubCommands();
            m_commands.Add(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public void RemoveCommand(Command<C> command)
        {
            if(command == null) throw new ArgumentException("command");

            m_commands.Remove(command);
        }
    }
}
