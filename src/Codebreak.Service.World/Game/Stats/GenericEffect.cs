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
        public int Total => Base + Items + Dons + Boosts;

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public EffectEnum EffectType => (EffectEnum)Id;

        /// <summary>
        /// 
        /// </summary>
        public int Id;
        public int Value1;
        public int Value2;
        public int Value3;
        public string Args;

        public int Base;
        public int Items;
        public int Dons;
        public int Boosts;

        /// <summary>
        /// 
        /// </summary>
        [ProtoIgnore]
        public int RandomJet => Util.NextJet(Value1, Value2);

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
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="args"></param>
        public GenericEffect(EffectEnum id, int value1, int value2, int value3, string args)
        {
            Id = (int)id;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
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
            Boosts += effect.Boosts;
            Dons += effect.Dons;
            Items += effect.Items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        public void UnMerge(GenericEffect effect)
        {
            Base -= effect.Base;
            Boosts -= effect.Boosts;
            Dons -= effect.Dons;
            Items -= effect.Items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        public void Merge(StatsType type, GenericEffect effect)
        {
            switch(type)
            {
                case StatsType.TYPE_BASE:
                    Base += effect.Value1;
                    break;

                case StatsType.TYPE_BOOST:
                    Boosts += effect.Value1;
                    break;

                case StatsType.TYPE_DON:
                    Dons += effect.Value1;
                    break;

                case StatsType.TYPE_ITEM:
                    Items += effect.Value1;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="effect"></param>
        public void UnMerge(StatsType type, GenericEffect effect)
        {
            switch (type)
            {
                case StatsType.TYPE_BASE:
                    Base -= effect.Value1;
                    break;

                case StatsType.TYPE_BOOST:
                    Boosts -= effect.Value1;
                    break;

                case StatsType.TYPE_DON:
                    Dons -= effect.Value1;
                    break;

                case StatsType.TYPE_ITEM:
                    Items -= effect.Value1;
                    break;
            }
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
                + Value1.ToString("x") + '#'
                + Value2.ToString("x") + '#'
                + Value3.ToString("x") + '#'
                + Args;
        }
    }
}
