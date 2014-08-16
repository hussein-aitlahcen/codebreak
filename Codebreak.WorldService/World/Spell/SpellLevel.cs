using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Spell
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public sealed class SpellLevel
    {
        public int SpellId;
        public int Level;
        public int APCost;
        public int MinPO;
        public int MaxPO;
        public int CSRate;
        public int ESCRate;
        public int InLine;
        public bool LOS;
        public bool EmptyCell;
        public bool AllowPOBoost;
        public int MaxLaunchPerTurn;
        public int MaxLaunchPerTarget;
        public int IsECSEndTurn;
        public int Cooldown;
        public int RequiredLevel;
        public string RangeType;
        public List<SpellEffect> Effects;
        public List<SpellEffect> CriticalEffects;
    }
}
