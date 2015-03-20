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
    public sealed class FightTurnProcessor : IDisposable
    {
        private List<FighterBase> m_fighterTurns = new List<FighterBase>();
        private FighterBase m_currentFighter;
        private int m_currentIndex;

        /// <summary>
        /// 
        /// </summary>
        public List<FighterBase> FighterOrder
        {
            get
            {
                return m_fighterTurns;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void SummonFighter(FighterBase fighter)
        {
            var index = m_fighterTurns.IndexOf(fighter.Invocator) + 1;
            if (index == 0)
                index = m_fighterTurns.Count;
            m_fighterTurns.Insert(index, fighter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveFighter(FighterBase fighter)
        {
            var index = m_fighterTurns.IndexOf(fighter);
            if (index == -1)
                return;
            m_fighterTurns.Remove(fighter);
            if(index <= m_currentIndex)
                m_currentIndex--;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighters"></param>
        public void InitTurns(IEnumerable<FighterBase> fighters)
        {
            var team1 = fighters.Where(Fighter => Fighter.Team.Id == 0).ToList();
            var team2 = fighters.Where(Fighter => Fighter.Team.Id == 1).ToList();

            team1 = team1.OrderByDescending(Fighter => Fighter.Initiative).ToList();
            team2 = team2.OrderByDescending(Fighter => Fighter.Initiative).ToList();

            foreach (var fighter in team1)
            {
                var index = team1.IndexOf(fighter);

                if (team2.Count - 1 >= index)
                {
                    var OppositeFighter = team2[index];

                    if (OppositeFighter.Initiative > fighter.Initiative)
                    {
                        m_fighterTurns.Add(OppositeFighter);
                        m_fighterTurns.Add(fighter);
                    }
                    else
                    {
                        m_fighterTurns.Add(fighter);
                        m_fighterTurns.Add(OppositeFighter);
                    }
                }
                else
                {
                    m_fighterTurns.Add(fighter);
                }
            }

            foreach (var Fighter in team2)
            {
                if (!m_fighterTurns.Contains(Fighter))
                    m_fighterTurns.Add(Fighter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FighterBase NextFighter
        {
            get
            {
                do
                {
                    if (m_currentFighter == null || m_currentIndex >= m_fighterTurns.Count - 1)
                    {
                        if (m_fighterTurns.Count == 0)
                            return null;

                        m_currentFighter = m_fighterTurns[0];
                        m_currentIndex = 0;
                    }
                    else
                    {
                        m_currentIndex++;
                        m_currentFighter = m_fighterTurns[m_currentIndex];
                    }
                }
                while (!m_currentFighter.CanBeginTurn);

                return m_currentFighter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_fighterTurns.Clear();
            m_fighterTurns = null;
            m_currentFighter = null;
        }
    }
}
