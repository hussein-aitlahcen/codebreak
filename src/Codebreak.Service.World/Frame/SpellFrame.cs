using System;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Network;

namespace Codebreak.Service.World.Frame
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpellFrame : FrameBase<SpellFrame, CharacterEntity, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<CharacterEntity, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'S':
                    switch (message[1])
                    {
                        case 'M':
                            return SpellMove;

                        case 'B':
                            return SpellBoost;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void SpellMove(CharacterEntity entity, string message)
        {
            var data = message.Substring(2).Split('|');
            if (data.Length != 2)
            {
                entity.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellId = int.Parse(data[0]);
            var position = int.Parse(data[1]);

            entity.AddMessage(() =>
                {
                    if (entity.Spells == null)
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (!entity.Spells.HasSpell(spellId))
                    {
                        entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    entity.Spells.MoveSpell(spellId, position);
                    entity.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void SpellBoost(CharacterEntity entity, string message)
        {
            var spellId = -1;
            if (!int.TryParse(message.Substring(2), out spellId))
            {
                entity.SafeDispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                return;
            }

            entity.AddMessage(() =>
            {
                if (entity.Spells == null)
                {
                    entity.Dispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                    return;
                }

                if (!entity.Spells.HasSpell(spellId))
                {
                    entity.Dispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                    return;
                }

                var spell = entity.Spells.GetSpellLevel(spellId);

                if (entity.SpellPoint < spell.Level)
                {
                    entity.Dispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                    return;
                }

                entity.Spells.LevelUp(spellId);
                entity.SpellPoint -= spell.Level;

                entity.Dispatch(WorldMessage.SPELL_UPGRADE_SUCCESS(spellId, spell.Level + 1));
                entity.Dispatch(WorldMessage.ACCOUNT_STATS(entity));
            });
        }
    }
}
