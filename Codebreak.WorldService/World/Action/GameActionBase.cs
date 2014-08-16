using Codebreak.WorldService.World.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Action
{
    public abstract class GameActionBase
    {
        public GameActionTypeEnum Type
        {
            get;
            private set;
        }

        public abstract bool CanAbort
        {
            get;
        }

        public EntityBase Entity
        {
            get;
            private set;
        }

        public bool IsFinished
        {
            get;
            protected set;
        }

        public GameActionBase(GameActionTypeEnum type, EntityBase entity)
        {
            Type = type;
            Entity = entity;
        }

        public virtual void Start()
        {
        }

        public virtual void Abort(params object[] args)
        {
            IsFinished = true;
        }

        public virtual void Stop(params object[] args)
        {
            IsFinished = true;
        }

        public virtual string SerializeAs_GameAction()
        {
            return "";
        }
    }
}
