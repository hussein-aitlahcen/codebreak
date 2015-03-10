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
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public abstract class Command<C> where C : CommandContext
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IList<SubCommand<C>> m_subCommands = new List<SubCommand<C>>(); 

        /// <summary>
        /// 
        /// </summary>
        public abstract string[] Aliases { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual bool CanExecute(C context)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Process(C context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Serialize(StringBuilder message, string parent = "")
        {
            if (m_subCommands.Count != 0)
                message.Append("[").Append(Aliases.First()).Append("]").Append('\n');
            else
                message.Append(parent).Append(Aliases.First()).Append(" : ").Append(Description).Append('\n');
            foreach(var subCommand in m_subCommands)            
                subCommand.Serialize(message, Aliases.First() + " ");            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Execute(C context)
        {
            if (CanExecute(context))
            {
                string word = context.TextCommandArgument.NextWord();
                if (word != null)
                {
                    foreach (var subCommand in m_subCommands)
                    {
                        if (subCommand.Aliases.Contains(word))
                        {
                            if (subCommand.CanExecute(context))
                            {
                                return subCommand.Execute(context);
                            }
                        }
                    }
                }

                context.TextCommandArgument.Position--;

                Process(context);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
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
                            m_subCommands.Add(subCommand);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="C"></typeparam>
    public abstract class SubCommand<C> : Command<C> where C : CommandContext
    {
    }
}
