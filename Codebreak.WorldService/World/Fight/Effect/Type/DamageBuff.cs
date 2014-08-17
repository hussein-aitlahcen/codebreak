﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Fight.Effect.Type
{
    public sealed class DamageBuff : BuffBase
    {
        public DamageBuff(CastInfos castInfos, FighterBase target)
            : base(castInfos, target, ActiveType.ACTIVE_ENDTURN, DecrementType.TYPE_ENDTURN)
        {
        }

        public override FightActionResultEnum ApplyEffect(ref int damageValue, CastInfos damageInfos = null)
        {
            var damage = CastInfos.RandomJet;

            return DamageEffect.ApplyDamages(CastInfos, Target, ref damage);
        }
    }
}
