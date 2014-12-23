using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Action.Type
{
    public class AttackAction : AIAction
    {
        private int TargetCell
        {
            get;
            set;
        }

        private int SpellId
        {
            get;
            set;
        }

        public AttackAction(AIFighter fighter)
            : base(fighter)
        {
            TargetCell = Fighter.Team.OpponentTeam.AliveFighters.OrderBy(f => f.Life).First().Cell.Id;
        }

        public override AIActionResult Initialize()
        {
            if (!Fighter.Spells.Empty)
            {
                var bests = Fighter.Spells.GetSpells().Where(spell => Fighter.Fight.CanLaunchSpell(Fighter, spell, spell.SpellId, Fighter.Cell.Id, TargetCell) == FightSpellLaunchResultEnum.RESULT_OK);
                if (bests.Count() > 0)
                {
                    SpellId = bests.OrderByDescending(spell => spell.Level * (spell.Effects.Count(effect => Effect.CastInfos.IsDamageEffect(effect.TypeEnum)) + 1)).First().SpellId;
                    Fighter.Fight.TryLaunchSpell(Fighter, SpellId, TargetCell, 500);
                    Timeout = 500;
                    return AIActionResult.Running;
                }
            }
  
            return AIActionResult.Failure;
        }

        public override AIActionResult Execute()
        {
            if (!Timedout)
                return AIActionResult.Running;

            return AIActionResult.Success;
        }
    }
}
