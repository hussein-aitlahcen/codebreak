using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DropManager : Singleton<DropManager>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prospection"></param>
        /// <param name="monster"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public List<InventoryItemDAO> GetDrops(long prospection, MonsterEntity monster, double rate)
        {
            var drops = new List<InventoryItemDAO>();
            foreach(var drop in monster.Grade.Template.Drops)
            {
                for (int i = 0; i < drop.Max; i++)
                {
                    if (TryDrop(prospection, drop, rate))
                    {
                        if(drop.ItemTemplate != null)
                        {
                            drops.Add(drop.ItemTemplate.Create());
                        }
                    }
                }
            }
            return drops;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prospection"></param>
        /// <param name="drop"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public bool TryDrop(long prospection, DropTemplateDAO drop, double rate)
        {
            if (drop.PPThreshold > prospection)
                return false;

            var realRate = drop.Rate * rate;
            var chance = Util.Next(0, 100);

            return chance <= realRate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighters"></param>
        /// <param name="totalProspection"></param>
        /// <param name="drops"></param>
        /// <returns></returns>
        public Dictionary<FighterBase, List<InventoryItemDAO>> Distribute(IEnumerable<FighterBase> fighters, long totalProspection, List<InventoryItemDAO> drops)
        {
            var distributed = new Dictionary<FighterBase, List<InventoryItemDAO>>();
            var orderedPlayers = fighters.OrderBy(player => player.Prospection);   
            foreach (var player in fighters)
                distributed.Add(player, new List<InventoryItemDAO>());
            while(drops.Count > 0)
            {
                foreach(var player in fighters)
                {
                    for (int i = drops.Count - 1; i > -1; i--)
                    {
                        var rand = Util.Next(0, 100);
                        var rate = (player.Prospection / (double)totalProspection) * 100;
                        if (rand < rate)
                        {
                            distributed[player].Add(drops[i]);
                            drops.RemoveAt(i);
                        }
                    }
                }
            }
            return distributed;
        }
    }
}
