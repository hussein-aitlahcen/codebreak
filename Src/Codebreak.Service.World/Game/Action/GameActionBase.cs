using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    public abstract class GameActionBase
    {
        public long Duration
        {
            get;
            protected set;
        }

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

        public GameActionBase(GameActionTypeEnum type, EntityBase entity, long duration = -1)
        {
            Type = type;
            Entity = entity;
            Duration = duration;
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
