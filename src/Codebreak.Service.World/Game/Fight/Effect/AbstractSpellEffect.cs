using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Effect
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractSpellEffect
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="castInfos"></param>
        /// <returns></returns>
        public abstract FightActionResultEnum ApplyEffect(CastInfos castInfos);
    }
}
