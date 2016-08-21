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
    public sealed class SpellCastManager : IDisposable
    {
        private Dictionary<int, List<SpellTarget>> m_targets = new Dictionary<int, List<SpellTarget>>();
        private Dictionary<int, SpellCooldown> m_cooldowns = new Dictionary<int, SpellCooldown>();

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            m_targets.Clear();
            m_cooldowns.Clear();
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
                if (m_cooldowns.ContainsKey(spellId))
                {
                    if (m_cooldowns[spellId] != null)
                    {
                        if (m_cooldowns[spellId].Cooldown > 0)
                            return false;
                    }
                }
            }

            if (spell.MaxLaunchPerTurn == 0 && spell.MaxLaunchPerTarget == 0)
                return true;

            if (spell.MaxLaunchPerTurn > 0)
            {
                if (m_targets.ContainsKey(spellId))
                {
                    if (m_targets[spellId].Count >= spell.MaxLaunchPerTurn)
                        return false;
                }
            }

            if (targetId == 0)
                return true;

            if (spell.MaxLaunchPerTarget > 0)
            {
                if (m_targets.ContainsKey(spellId))
                {
                    if (m_targets[spellId].Count(spellTarget => spellTarget.TargetId == targetId) >= spell.MaxLaunchPerTarget)
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
                if (!m_cooldowns.ContainsKey(spellId))
                {
                    m_cooldowns.Add(spellId, new SpellCooldown(spell.Cooldown));
                }
                else
                {
                    m_cooldowns[spellId].Cooldown = spell.Cooldown;
                }
            }

            if (spell.MaxLaunchPerTurn == 0 && spell.MaxLaunchPerTarget == 0)
                return;

            if (!m_targets.ContainsKey(spellId))
                m_targets.Add(spellId, new List<SpellTarget>());
            m_targets[spellId].Add(new SpellTarget(targetId));
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndTurn()
        {
            foreach (var target in m_targets.Values)
                target.Clear();

            foreach (var cooldown in m_cooldowns.Values)
                cooldown.Decrement();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_targets.Clear();
            m_cooldowns.Clear();
            m_targets = null;
            m_cooldowns = null;
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
