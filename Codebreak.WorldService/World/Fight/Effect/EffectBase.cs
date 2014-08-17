using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Fight.Effect
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EffectBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public abstract FightActionResultEnum ApplyEffect(CastInfos castInfos);
    }
}
