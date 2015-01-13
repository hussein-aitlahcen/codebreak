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
        private List<Func<ChallengeBase>> m_challengeGenerator;
        
        /// <summary>
        /// 
        /// </summary>
        public ChallengeManager()
        {
            m_challengeGenerator = new List<Func<ChallengeBase>>();
            m_challengeGenerator.Add(() => new ZombieChallenge());
            m_challengeGenerator.Add(() => new AnachoriteChallenge());
            m_challengeGenerator.Add(() => new AbnegationChallenge());
            m_challengeGenerator.Add(() => new BarbarianChallenge());
            m_challengeGenerator.Add(() => new CirculateChallenge());
            m_challengeGenerator.Add(() => new TightsChallenge());
            m_challengeGenerator.Add(() => new AppointedVoluntaryChallenge());
            m_challengeGenerator.Add(() => new BoldChallenge());
            m_challengeGenerator.Add(() => new StatueChallenge());
            m_challengeGenerator.Add(() => new ReprieveChallenge());
            m_challengeGenerator.Add(() => new SightLostChallenge());
            m_challengeGenerator.Add(() => new SurvivorChallenge());
            m_challengeGenerator.Add(() => new PetulantChallenge());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ChallengeBase> Generate(int length)
        {
            List<ChallengeBase> challenges = new List<ChallengeBase>();
            while (challenges.Count < length)
            {
                var challenge = m_challengeGenerator[Util.Next(0, m_challengeGenerator.Count)]();
                if (challenges.Any(chall => chall.Id == challenge.Id))
                    continue;
                challenges.Add(challenge);
            }
            return challenges;
        }
    }
}
