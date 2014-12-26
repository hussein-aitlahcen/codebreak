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
        private List<FighterBase> _fighterTurns = new List<FighterBase>();
        private FighterBase _currentFighter;
        private int _currentIndex;

        /// <summary>
        /// 
        /// </summary>
        public List<FighterBase> FighterOrder
        {
            get
            {
                return _fighterTurns;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        public void RemoveFighter(FighterBase fighter)
        {
            if (_fighterTurns.Contains(fighter))
                _fighterTurns.Remove(fighter);
            if (_currentFighter == fighter)
                _currentIndex--;
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
                        _fighterTurns.Add(OppositeFighter);
                        _fighterTurns.Add(fighter);
                    }
                    else
                    {
                        _fighterTurns.Add(fighter);
                        _fighterTurns.Add(OppositeFighter);
                    }
                }
                else
                {
                    _fighterTurns.Add(fighter);
                }
            }

            foreach (var Fighter in team2)
            {
                if (!_fighterTurns.Contains(Fighter))
                    _fighterTurns.Add(Fighter);
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
                    if (_currentFighter == null || _currentIndex >= _fighterTurns.Count - 1)
                    {
                        _currentFighter = _fighterTurns[0];
                        _currentIndex = 0;
                    }
                    else
                    {
                        _currentIndex++;
                        _currentFighter = _fighterTurns[_currentIndex];
                    }
                }
                while (!_currentFighter.CanBeginTurn);

                return _currentFighter;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _fighterTurns.Clear();
            _fighterTurns = null;
            _currentFighter = null;
        }
    }
}
