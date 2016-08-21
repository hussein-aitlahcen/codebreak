using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Database.Structure;

namespace Codebreak.Service.World.Game.Fight.Ending
{
    public sealed class EndingArguments<T> where T : AbstractFighter
    {
        public AbstractFight Fight { get; }
        public List<AbstractFighter> Droppers { get; }
        public List<T> Losers { get; }
        public long DroppersTotalPP { get; }
        public List<ItemDAO> ItemLoot { get; }
        public long KamasLoot { get; } 
        public EndingArguments(AbstractFight fight, List<AbstractFighter> droppers, List<T> losers, long droppersTotalPP, List<ItemDAO> itemLoot, long kamasLoot)
        {
            Fight = fight;
            Droppers = droppers;
            Losers = losers;
            DroppersTotalPP = droppersTotalPP;
            ItemLoot = itemLoot;
            KamasLoot = kamasLoot;
        }
    }
}
