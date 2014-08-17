using Codebreak.Service.World.Game.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpellCastManager
    {
        private Dictionary<int, List<SpellTarget>> _targets = new Dictionary<int, List<SpellTarget>>();
        private Dictionary<int, SpellCooldown> _cooldowns = new Dictionary<int, SpellCooldown>();

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _targets.Clear();
            _cooldowns.Clear();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="spellId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        public bool CanLaunchSpell(SpellLevel spell, int spellId, long targetId)
        {
            if (spell.Cooldown > 0)
            {
                if (_cooldowns.ContainsKey(spellId))
                {
                    if (_cooldowns[spellId] != null)
                    {
                        if (_cooldowns[spellId].Cooldown > 0)
                            return false;
                    }
                }
            }

            if (spell.MaxLaunchPerTurn == 0 && spell.MaxLaunchPerTarget == 0)
                return true;

            if (spell.MaxLaunchPerTurn > 0)
            {
                if (_targets.ContainsKey(spellId))
                {
                    if (_targets[spellId].Count >= spell.MaxLaunchPerTurn)
                        return false;
                }
            }

            if (spell.MaxLaunchPerTarget > 0)
            {
                if (_targets.ContainsKey(spellId))
                {
                    if (_targets[spellId].Count(spellTarget => spellTarget.TargetId == targetId) >= spell.MaxLaunchPerTarget)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="spellId"></param>
        /// <param name="targetId"></param>
        public void Actualize(SpellLevel spell, int spellId, long targetId)
        {
            if (spell.Cooldown > 0)
            {
                if (!_cooldowns.ContainsKey(spellId))
                {
                    _cooldowns.Add(spellId, new SpellCooldown(spell.Cooldown));
                }
                else
                {
                    _cooldowns[spellId].Cooldown = spell.Cooldown;
                }
            }

            if (spell.MaxLaunchPerTurn == 0 && spell.MaxLaunchPerTarget == 0)
                return;

            if (spell.MaxLaunchPerTurn > 0)
            {
                if (!_targets.ContainsKey(spellId))
                {
                    _targets.Add(spellId, new List<SpellTarget>());
                }
            }

            if (spell.MaxLaunchPerTarget > 0)
            {
                if (_targets.ContainsKey(spellId))
                {
                    _targets[spellId].Add(new SpellTarget(targetId));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndTurn()
        {
            foreach (var target in _targets.Values)
                target.Clear();

            foreach (var cooldown in _cooldowns.Values)
                cooldown.Decrement();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SpellCooldown
    {
        /// <summary>
        /// 
        /// </summary>
        public int Cooldown
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cooldown"></param>
        public SpellCooldown(int cooldown)
        {
            Cooldown = Cooldown;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Decrement()
        {
            Cooldown--;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SpellTarget
    {
        /// <summary>
        /// 
        /// </summary>
        public long TargetId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetId"></param>
        public SpellTarget(long targetId)
        {
            TargetId = targetId;
        }
    }
}
