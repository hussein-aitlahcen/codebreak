using Codebreak.Service.World.Game.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Action
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameFightSpellAction : GameFightActionBase
    {
        /// <summary>
        /// 
        /// </summary>
        public System.Action Callback
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellLevel
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Sprite
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string SpriteInfos
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fighter"></param>
        /// <param name="callback"></param>
        public GameFightSpellAction(FighterBase fighter, int cellId, int spellId, int spellLevel, string sprite, string spriteInfos, long duration, System.Action callback)
            : base(GameActionTypeEnum.FIGHT_SPELL_LAUNCH, fighter, duration)
        {
            Callback = callback;
            CellId = cellId;
            SpellId = spellId;
            SpellLevel = spellLevel;
            Sprite = sprite;
            SpriteInfos = spriteInfos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public override void Stop(params object[] args)
        {
            Callback();

            base.Stop(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string SerializeAs_GameAction()
        {
            return SpellId + "," + CellId + "," + Sprite + "," + SpellLevel + "," + SpriteInfos;
        }
    }
}
