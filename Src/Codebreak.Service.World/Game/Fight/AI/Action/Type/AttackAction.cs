using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.AI.Action.Type
{
    public enum AttackStateEnum
    { 
        STATE_CALCULATE_CELLS,
        STATE_CALCULATE_EFFECT_TARGETS,
        STATE_CALCULATE_BEST_SPELL,
        STATE_LAUNCH_ATTACK,
        STATE_ATTACKING,
    }

    public class AttackAction : AIAction
    {
        private AttackStateEnum AttackState
        {
            get;
            set;
        }

        private Dictionary<int, List<SpellLevel>> CastCellList
        {
            get;
            set;
        }

        private Dictionary<int, Dictionary<int, Dictionary<SpellEffect, List<FighterBase>>>> TargetList
        {
            get;
            set;
        }

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
            AttackState = AttackStateEnum.STATE_CALCULATE_CELLS;
        }

        public override AIActionResult Initialize()
        {
            TargetCell = 0;
            SpellId = 0;
            AttackState = AttackStateEnum.STATE_CALCULATE_CELLS;

            return Fighter.AP > 0 && Fighter.Spells.GetSpells().Any(spell => spell.APCost <= Fighter.AP) ? AIActionResult.RUNNING : AIActionResult.FAILURE;
        }

        public override AIActionResult Execute()
        {
            switch (AttackState)
            {
                case AttackStateEnum.STATE_CALCULATE_CELLS:
                    CastCellList = new Dictionary<int,List<SpellLevel>>();
                    foreach(var spellLevel in Fighter.Spells.GetSpells())
                    {
                        foreach (var castCell in CellZone.GetCircleCells(Map, Fighter.Cell.Id, spellLevel.MaxPO))
                        {
                            if (Fight.CanLaunchSpell(Fighter, spellLevel, spellLevel.SpellId, Fighter.Cell.Id, castCell) == FightSpellLaunchResultEnum.RESULT_OK)
                            {
                                if (!CastCellList.ContainsKey(castCell))
                                    CastCellList.Add(castCell, new List<SpellLevel>());
                                CastCellList[castCell].Add(spellLevel);
                            }
                        }
                    }

                    if (CastCellList.Count == 0)
                        return AIActionResult.FAILURE;

                    AttackState = AttackStateEnum.STATE_CALCULATE_EFFECT_TARGETS;
                    return AIActionResult.RUNNING;

                case AttackStateEnum.STATE_CALCULATE_EFFECT_TARGETS:
                    TargetList = new Dictionary<int, Dictionary<int, Dictionary<SpellEffect, List<FighterBase>>>>();
                    foreach(var castInfos in CastCellList)
                    {
                        var castCell = castInfos.Key;
                        TargetList.Add(castCell, new Dictionary<int, Dictionary<SpellEffect, List<FighterBase>>>());
                        foreach(var spellLevel in castInfos.Value)
                        {
                            if (spellLevel == null || spellLevel.Effects == null)
                                continue;

                            TargetList[castCell].Add(spellLevel.SpellId, new Dictionary<SpellEffect, List<FighterBase>>());

                            int effectIndex = 0;
                            foreach (var effect in spellLevel.Effects)
                            {
                                TargetList[castCell][spellLevel.SpellId].Add(effect, new List<FighterBase>());

                                var targetType = spellLevel.Targets != null ? spellLevel.Targets.Length > effectIndex ? spellLevel.Targets[effectIndex] : -1 : -1;

                                if (effect.TypeEnum != EffectEnum.UseGlyph && effect.TypeEnum != EffectEnum.UseTrap)
                                {
                                    foreach (var currentCellId in CellZone.GetCells(Map, castCell, Fighter.Cell.Id, spellLevel.RangeType))
                                    {
                                        var fightCell = Fight.GetCell(currentCellId);
                                        if (fightCell != null)
                                        {
                                            foreach (var fighterObject in fightCell.FightObjects.OfType<FighterBase>())
                                            {
                                                if (targetType != -1)
                                                {
                                                    if (((targetType & 1) == 1) && Fighter.Team != fighterObject.Team)
                                                        continue;
                                                    if ((((targetType >> 1) & 1) == 1) && Fighter == fighterObject)
                                                        continue;
                                                    if ((((targetType >> 2) & 1) == 1) && Fighter.Team != fighterObject.Team)
                                                        continue;
                                                    if (((((targetType >> 3) & 1) == 1) && (Fighter.Invocator == null)))
                                                        continue;
                                                    if (((((targetType >> 4) & 1) == 1) && (Fighter.Invocator != null)))
                                                        continue;
                                                    if (((((targetType >> 5) & 1) == 1) && (Fighter.Id != fighterObject.Id)))
                                                    {
                                                        if (!TargetList[castCell][spellLevel.SpellId][effect].Contains(Fighter))
                                                            TargetList[castCell][spellLevel.SpellId][effect].Add(Fighter);
                                                        continue;
                                                    }
                                                }

                                                if (!TargetList[castCell][spellLevel.SpellId][effect].Contains(fighterObject))
                                                    TargetList[castCell][spellLevel.SpellId][effect].Add(fighterObject);
                                            }
                                        }
                                    }
                                }
                                effectIndex++;
                            }
                        }
                    }
                        
                    AttackState = AttackStateEnum.STATE_CALCULATE_BEST_SPELL;

                    return AIActionResult.RUNNING;

                case AttackStateEnum.STATE_CALCULATE_BEST_SPELL:
                    int bestScore = -1;
                    foreach(var target in TargetList)
                    {
                        var castCell = target.Key;

                        foreach (var spell in target.Value)
                        {
                            var spellId = spell.Key;
                            var currentScore = -1;
                            foreach (var levelInfos in spell.Value)
                            {
                                var effect = levelInfos.Key;
                                foreach (var fighter in levelInfos.Value)
                                {
                                    if (Effect.CastInfos.IsDamageEffect(effect.TypeEnum))
                                    {
                                        if (fighter.Team.Id != Fighter.Team.Id)
                                            currentScore += 10 + effect.Value1 + effect.Value2 + effect.Value3;
                                        else
                                            currentScore -= 10 + effect.Value1 + effect.Value2 + effect.Value3;
                                    }
                                    else if (Effect.CastInfos.IsMalusEffect(effect.TypeEnum))
                                    {
                                        if (fighter.Team.Id != Fighter.Team.Id)
                                            currentScore += 5 + effect.Value1 + effect.Value2 + effect.Value3;
                                        else
                                            currentScore -= 5 + effect.Value1 + effect.Value2 + effect.Value3; ;
                                    }
                                    else if(Effect.CastInfos.IsBonusEffect(effect.TypeEnum))
                                    {
                                        if (fighter.Team.Id != Fighter.Team.Id)
                                            currentScore -= effect.Value1 + effect.Value2 + effect.Value3;
                                        else
                                            if (effect.TypeEnum == EffectEnum.Heal)
                                                currentScore += (effect.Value1 + effect.Value2 + effect.Value3) * (1 + ((fighter.MaxLife / 100) * fighter.Life));
                                            else
                                                currentScore += effect.Value1 + effect.Value2 + effect.Value3;
                                    }
                                }
                            }

                            if (currentScore > bestScore)
                            {
                                bestScore = currentScore;
                                SpellId = spellId;
                                TargetCell = castCell;
                            }
                        }
                    }

                    if (SpellId == 0)
                        return AIActionResult.FAILURE;
                    
                    AttackState = AttackStateEnum.STATE_LAUNCH_ATTACK;

                    return AIActionResult.RUNNING;

                case AttackStateEnum.STATE_LAUNCH_ATTACK:
                                        
                    Fight.TryLaunchSpell(Fighter, SpellId, TargetCell, 1000);
                    Timeout = 1000;

                    AttackState = AttackStateEnum.STATE_ATTACKING;

                    return AIActionResult.RUNNING;

                case AttackStateEnum.STATE_ATTACKING:
                    if (!Timedout)
                        return AIActionResult.RUNNING;
                    
                    return Initialize();

                default:
                    throw new Exception("AI Attack action invalid state.");
            }
        }
    }
}
