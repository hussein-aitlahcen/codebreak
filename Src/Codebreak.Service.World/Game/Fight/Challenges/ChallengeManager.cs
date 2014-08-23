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
    public enum ChallengeTypeEnum
    {
        ZOMBIE = 1,
        STATUE = 2,
        APPOINTED_VOLUNTARY = 3, // designé volontaire
        REPRIEVE = 4, // sursis
        PEELER = 5, // econome
        VERSATILE = 6,
        BARBARIRAN = 9, // barbare
        CIRCULATE = 21,
        LOST_SIGHT = 23, // perdu de vu
        SURVIVOR = 33,
        BOLD = 36, // hardi
        TIGHTS = 37, // collant
        ANACHORITE = 39, // anachorite
        PETULANT = 41,
        ABNEGATION = 43,
    }

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
            _challengeGenerator.Add(() => new AnachoriteChallenge());
            _challengeGenerator.Add(() => new AbnegationChallenge());
            _challengeGenerator.Add(() => new BarbarianChallenge());
            _challengeGenerator.Add(() => new CirculateChallenge());
            _challengeGenerator.Add(() => new TightsChallenge());
            _challengeGenerator.Add(() => new AppointedVoluntaryChallenge());
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
