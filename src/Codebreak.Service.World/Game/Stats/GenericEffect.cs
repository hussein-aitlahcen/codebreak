using Codebreak.Service.World.Game.Spell;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Stats
{
    /// <summary>
    /// Effet possible de statistique
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class GenericEffect
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public int Total
        {
            get
            {
                return Base + Items + Dons + Boosts;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public EffectEnum EffectType
        {
            get
            {
                return (EffectEnum)Id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id;
        public int Min;
        public int Max;
        public string Args;

        public int Base;
        public int Items;
        public int Dons;
        public int Boosts;

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public int RandomJet
        {
            get
            {
                return Util.NextJet(Min, Max);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="baseValue"></param>
        /// <param name="items"></param>
        /// <param name="dons"></param>
        /// <param name="boosts"></param>
        public GenericEffect(EffectEnum id, int baseValue = 0, int items = 0, int dons = 0, int boosts = 0)
        {
            Id = (int)id;
            Base = baseValue;
            Items = items;
            Dons = dons;
            Boosts = boosts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="args"></param>
        public GenericEffect(EffectEnum id, int min, int max, string args = "0")
        {
            Id = (int)id;
            Min = min;
            Max = max;
            Args = args;
        }

        /// <summary>
        /// 
        /// </summary>
        public GenericEffect()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        public void Merge(GenericEffect effect)
        {
            Base += effect.Base;
            Items += effect.RandomJet;
            Dons += effect.Dons;
            Boosts += effect.Boosts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        public void UnMerge(GenericEffect effect)
        {
            Base -= effect.Base;
            Items -= effect.RandomJet;
            Dons -= effect.Dons;
            Boosts -= effect.Boosts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Base + "," + Items + "," + Dons + "," + Boosts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToItemString()
        {
            return Id.ToString("x") + '#'
                + Min.ToString("x") + '#'
                + Max.ToString("x") + '#'
                + Args + '#'
                + "0d0+0";
        }
    }
}
