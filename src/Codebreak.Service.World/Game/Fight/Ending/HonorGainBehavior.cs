using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Ending
{
    public sealed class HonorGainBehavior : AbstractEndingBehavior
    {
        public override void Execute(AbstractFight fight)
        {
            var winnersTotalLevel = fight.WinnerFighters.Sum(fighter => fighter.Level);
            var losersTotalLevel = fight.LoserFighters.Sum(fighter => fighter.Level);

            foreach (var fighter in fight.WinnerFighters)
            {
                var honour = 0;
                var dishonour = 0;
                if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    var player = (CharacterEntity)fighter;

                    if (player.AlignmentId != (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL)
                    {
                        if (!fight.IsNeutralAgression || player.Team.AlignmentId == (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL)
                        {
                            honour = Util.CalculWinHonor(player.Level, winnersTotalLevel, losersTotalLevel);
                            player.SubstractDishonour(1);
                        }
                        else
                            dishonour = 1;
                        player.AddHonour(honour);
                    }
                }
                fight.Result.AddResult(fighter, FightEndTypeEnum.END_WINNER, false, 0, 0, honour, dishonour);
            }

            foreach (var fighter in fight.LoserFighters)
            {
                var honour = 0;
                var dishonour = 0;
                if (fighter.Type == EntityTypeEnum.TYPE_CHARACTER)
                {
                    var player = (CharacterEntity)fighter;
                    if (player.AlignmentId != (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL)
                    {
                        if (!fight.IsNeutralAgression || player.Team.AlignmentId != (int)AlignmentTypeEnum.ALIGNMENT_NEUTRAL)
                            honour = Util.CalculWinHonor(player.Level, winnersTotalLevel, losersTotalLevel);
                        player.SubstractHonour(honour);
                    }
                    else
                        dishonour = 1;
                }
                fight.Result.AddResult(fighter, FightEndTypeEnum.END_LOSER, false, 0, 0, -honour, dishonour);
            }
        }
    }
}
