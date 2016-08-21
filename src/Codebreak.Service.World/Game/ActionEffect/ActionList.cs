using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.ActionEffect
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionEntry
    {
        public EffectEnum Effect { get; }
        public Dictionary<string, string> Parameters { get; }
        public ActionEntry(EffectEnum effect, Dictionary<string, string> parameters)
        {
            Effect = effect;
            Parameters = parameters;
        }
        public static ActionEntry Deserialize(string data)
        {
            var splitted = data.Split(':');
            var effect = (EffectEnum)int.Parse(splitted[0]);
            var parameters = new Dictionary<string, string>();
            foreach (var parameter in splitted[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var splittedParam = parameter.Split('=');
                var key = splittedParam[0];
                var value = splittedParam[1];
                parameters.Add(key, value);
            }
            return new ActionEntry(effect, parameters);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ActionList : List<ActionEntry>
    {
        public static ActionList Deserialize(string data)
        {
            var list = new ActionList();
            list.AddRange(data.Split('|').Select(ActionEntry.Deserialize));
            return list;
        }
    }
}
