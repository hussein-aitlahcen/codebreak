using Codebreak.Framework.Generic;
using Codebreak.Service.World.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Fight.Challenges
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChallengeManager : Singleton<ChallengeManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private List<Func<ChallengeBase>> _challengeGenerator;
        
        /// <summary>
        /// 
        /// </summary>
        public ChallengeManager()
        {
            _challengeGenerator = new List<Func<ChallengeBase>>();
            _challengeGenerator.Add(() => new ZombieChallenge());    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChallengeBase Generate()
        {
            return _challengeGenerator[Util.Next(0, _challengeGenerator.Count)]();
        }
    }
}
