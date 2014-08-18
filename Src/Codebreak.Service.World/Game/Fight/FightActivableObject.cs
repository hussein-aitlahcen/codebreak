using Codebreak.Service.World.Game.Fight.Effect;
using Codebreak.Service.World.Game.Map;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;
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
    public abstract class FightActivableObject : IFightObstacle
    {
        /// <summary>
        /// 
        /// </summary>
        public FightObstacleTypeEnum ObstacleType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ActiveType ActivationType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanGoThrough
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CanStack
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Duration
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Length
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Hide
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ActionId
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FightCell> AffectedCells
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Activated
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public FightCell Cell
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FighterBase> Targets
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        protected FightBase _fight;
        protected FighterBase _caster;
        protected SpellTemplate _actionSpell;
        protected SpellLevel _actionEffect;
        protected int _spellId;

        /// <summary>
        /// 
        /// </summary>
        public abstract void AppearForAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        public abstract void Appear(MessageDispatcher dispatcher);

        /// <summary>
        /// 
        /// </summary>
        public abstract void DisappearForAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fight"></param>
        /// <param name="caster"></param>
        /// <param name="effect"></param>
        /// <param name="cell"></param>
        /// <param name="duration"></param>
        /// <param name="actionId"></param>
        /// <param name="hide"></param>
        public FightActivableObject(FightObstacleTypeEnum type, ActiveType activeType, FightBase fight, FighterBase caster, CastInfos castInfos, int cell, int duration, int actionId, bool canGoThrough, bool canStack, bool hide = false)
        {
            _fight = fight;
            _caster = caster;
            _spellId = castInfos.SpellId;
            _actionSpell = SpellManager.Instance.GetTemplate(castInfos.Value1);
            _actionEffect = _actionSpell.GetLevel(castInfos.Value2);

            Cell = fight.GetCell(cell);
            ObstacleType = type;
            ActivationType = activeType;
            CanGoThrough = canGoThrough;
            CanStack = canStack;
            Color = castInfos.Value3;
            Targets = new List<FighterBase>();
            Length = Pathfinding.GetDirection(castInfos.RangeType[1]);
            AffectedCells = new List<FightCell>();
            Duration = duration;
            ActionId = actionId;
            Hide = hide;
                        
            foreach(var effect in _actionEffect.Effects)
            {
                if(CastInfos.IsDamageEffect(effect.TypeEnum))
                {
                    Priority--;
                }
            }

            // On ajout l'objet a toutes les cells qu'il affecte
            foreach (var cellId in CellZone.GetCircleCells(fight.Map, cell, Length))
            {
                var fightCell = _fight.GetCell(cellId);
                if (fightCell != null)
                {
                    fightCell.AddObject(this);

                    AffectedCells.Add(fightCell);
                }
            }

            if (Hide)
                Appear(caster.Team);
            else
                AppearForAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void LoadTargets(FighterBase target)
        {
            if(!Targets.Contains(target))
                Targets.Add(target);

            switch (ActivationType)
            {
                case ActiveType.ACTIVE_ENDMOVE:
                    foreach (var cell in AffectedCells)
                    {
                        Targets.AddRange(cell.FightObjects.OfType<FighterBase>().Where(fighter => !Targets.Contains(fighter)));
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activator"></param>
        public void Activate(FighterBase activator)
        {
            Activated = true;

            _fight.CurrentProcessingFighter = activator;
            _fight.Dispatch(WorldMessage.GAME_ACTION(ActionId, activator.Id, _spellId + "," + Cell.Id + "," + _actionSpell.Sprite + "," + _actionEffect.Level + ",1," + _caster.Id));

            foreach (var target in Targets)
            {
                if (!target.IsFighterDead)
                {
                    foreach (var effect in _actionEffect.Effects)
                    {
                        _fight.AddProcessingTarget(new CastInfos(
                                            effect.TypeEnum,
                                            _spellId,
                                            Cell.Id,
                                            effect.Value1,
                                            effect.Value2,
                                            effect.Value3,
                                            effect.Chance,
                                            effect.Duration,
                                            _caster,
                                            target,targetKnownCellId: target.Cell.Id,
                                            isTrap: ObstacleType == FightObstacleTypeEnum.TYPE_TRAP)
                                         );
                    }
                }
            }

            Targets.Clear();

            if (ObstacleType == FightObstacleTypeEnum.TYPE_TRAP)
                Remove();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DecrementDuration()
        {
            Duration--;

            if (Duration <= 0)
                Remove();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Remove()
        {
            DisappearForAll();

            foreach (var cell in AffectedCells)
            {
                cell.RemoveObject(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int IComparable<IFightObstacle>.CompareTo(IFightObstacle obj)
        {
            return Priority.CompareTo(obj.Priority);
        }
    }
}
