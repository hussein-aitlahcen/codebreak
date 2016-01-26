using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Stats
{
    public sealed class RandomEffect
    {
        public EffectEnum Type { get; }
        public int Minimum { get; }
        public int Maximum { get; }
        public int Random { get { return Util.NextJet(Minimum, Maximum); } }
        public RandomEffect(EffectEnum type, int min, int max)
        {
            Type = type;
            Minimum = min;
            Maximum = max;
        }
        public void Serialize(StringBuilder sb)
        {
            sb.Append(((int)Type).ToString("X2")).Append('#');
            sb.Append(Minimum.ToString("X2")).Append('#');
            sb.Append(Maximum.ToString("X2"));
        }
        public static RandomEffect Deserialize(string data)
        {
            var splitted = data.Split('#');
            var effect = (EffectEnum)int.Parse(splitted[0], System.Globalization.NumberStyles.HexNumber);
            var min = int.Parse(splitted[1], System.Globalization.NumberStyles.HexNumber);
            var max = int.Parse(splitted[2], System.Globalization.NumberStyles.HexNumber);
            return new RandomEffect(effect, min, max);
        }        
    }
    public sealed class RandomStatistics : List<RandomEffect>
    {
        public string Serialize()
        {
            var sb = new StringBuilder();
            foreach (var effect in this)
            {
                effect.Serialize(sb);
                sb.Append(',');
            }
            return sb.ToString();
        }
        public static RandomStatistics Deserialize(string data)
        {
            var statistics = new RandomStatistics();
            if(!string.IsNullOrWhiteSpace(data))
            {
                foreach(var effect in data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    statistics.Add(RandomEffect.Deserialize(effect));
                }
            }
            return statistics;
        }
    }
}
