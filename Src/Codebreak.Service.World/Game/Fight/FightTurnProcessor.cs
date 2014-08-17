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
    public sealed class FightTurnProcessor
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Fighters"></param>
        public void InitTurns(IEnumerable<FighterBase> Fighters)
        {
            var Team1 = Fighters.Where(Fighter => Fighter.Team.Id == 0).ToList();
            var Team2 = Fighters.Where(Fighter => Fighter.Team.Id == 1).ToList();

            Team1 = Team1.OrderByDescending(Fighter => Fighter.Initiative).ToList();
            Team2 = Team2.OrderByDescending(Fighter => Fighter.Initiative).ToList();

            foreach (var Fighter in Team1)
            {
                var FIndex = Team1.IndexOf(Fighter);

                if (Team2.Count - 1 >= FIndex)
                {
                    var OppositeFighter = Team2[FIndex];

                    if (OppositeFighter.Initiative > Fighter.Initiative)
                    {
                        _fighterTurns.Add(OppositeFighter);
                        _fighterTurns.Add(Fighter);
                    }
                    else
                    {
                        _fighterTurns.Add(Fighter);
                        _fighterTurns.Add(OppositeFighter);
                    }
                }
                else
                {
                    _fighterTurns.Add(Fighter);
                }
            }

            foreach (var Fighter in Team2)
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
    }
}
