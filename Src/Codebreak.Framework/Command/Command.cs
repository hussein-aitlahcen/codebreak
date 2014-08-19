using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Framework.Command
{
    public abstract class Command<C> where C : CommandContext
    {
        private readonly IList<SubCommand<C>> _subCommands = new List<SubCommand<C>>(); 

        public abstract string[] Aliases { get; }

        public abstract string Description { get; }

        protected virtual bool CanExecute(C context)
        {
            return true;
        }

        protected virtual void Process(C context)
        {
        }

        public void Execute(C context)
        {
            if (CanExecute(context))
            {
                string word = context.TextCommandArgument.NextWord();
                if (word != null)
                {
                    foreach (var subCommand in _subCommands)
                    {
                        if (subCommand.Aliases.Contains(word))
                        {
                            subCommand.Execute(context);
                            return;
                        }
                    }
                    context.TextCommandArgument.Position -= word.Length;
                }

                Process(context);
            }
        }

        internal void RegisterNestedSubCommands()
        {
            var type = GetType();
            var nestedClasses = type.GetNestedTypes(BindingFlags.Public);
            if (nestedClasses.Length > 0)
            {
                foreach (var nestedType in nestedClasses)
                {
                    if (nestedType.IsSubclassOf(typeof(SubCommand<C>)) && !nestedType.IsAbstract)
                    {
                        var subCommand = Activator.CreateInstance(nestedType) as SubCommand<C>;
                        if(subCommand != null)
                            _subCommands.Add(subCommand);
                    }
                }
            }
        }
    }

    public abstract class SubCommand<C> : Command<C> where C : CommandContext
    {
    }
}
