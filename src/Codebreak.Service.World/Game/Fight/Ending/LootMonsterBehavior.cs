using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.Game.Fight.Ending
{
    public sealed class LootMonsterBehavior : AbstractLootBehavior<MonsterEntity>
    {
        protected override long GetAdditionalKamas(AbstractFight fight)
        {
            var monsterFight = fight as MonsterFight;
            if (monsterFight != null)
                if (fight.WinnerTeam == fight.Team0)
                    return monsterFight.MonsterGroup.Kamas;
            return 0;
        }

        protected override IEnumerable<ItemDAO> GetAdditionalLoot(AbstractFight fight)
        {
             var monsterFight = fight as MonsterFight;
            if (monsterFight != null)
                if (fight.WinnerTeam == fight.Team0)
                    return monsterFight.MonsterGroup.Inventory.RemoveItems();
            return Enumerable.Empty<ItemDAO>();
        }

        protected override IEnumerable<AbstractFighter> GetAdditionalDroppers(AbstractFight fight)
        {
            if (fight.Map.SubArea.TaxCollector != null)
                yield return fight.Map.SubArea.TaxCollector;
        }

        protected override long GetTargetKamas(EndingArguments<MonsterEntity> arguments, MonsterEntity fighter)
        {
            return (long)Math.Round(
                    Util.Next(fighter.Grade.Template.MinKamas, fighter.Grade.Template.MaxKamas)
                    * WorldConfig.RATE_KAMAS
                    * arguments.Fight.ChallengeLootBonus);
        }

        protected override IEnumerable<ItemDAO> GetTargetItems(EndingArguments<MonsterEntity> arguments, MonsterEntity fighter)
        {
            return DropManager.Instance.GetDrops
                (
                    arguments.DroppersTotalPP,
                    fighter,
                    WorldConfig.RATE_DROP * arguments.Fight.ChallengeLootBonus
                );
        }

        protected override long GetExperienceWon(EndingArguments<MonsterEntity> arguments, AbstractFighter fighter)
        {
            var monsterFight = arguments.Fight as MonsterFight;
            switch (fighter.Type)
            {
                case EntityTypeEnum.TYPE_CHARACTER:
                    return Util.CalculPVMExperience(
                                   arguments.Losers,
                                   arguments.Droppers,
                                   fighter.Level,
                                   fighter.Statistics.GetTotal(EffectEnum.AddWisdom),
                                   arguments.Fight.ChallengeXpBonus,
                                   monsterFight?.MonsterGroup.AgeBonus ?? 0);

                case EntityTypeEnum.TYPE_TAX_COLLECTOR:
                    return Util.CalculPVMExperienceTaxCollector(
                                   arguments.Losers,
                                   arguments.Droppers, 
                                   fighter.Level,
                                   fighter.Statistics.GetTotal(EffectEnum.AddWisdom),
                                   arguments.Fight.ChallengeXpBonus,
                                   monsterFight?.MonsterGroup.AgeBonus ?? 0);

            }
            return 0;
        }

        protected override long GetKamasWon(EndingArguments<MonsterEntity> arguments, AbstractFighter fighter)
        {
            return Util.CalculPVMKamas(arguments.KamasLoot, fighter.Prospection, arguments.DroppersTotalPP);
        }
    }
}
