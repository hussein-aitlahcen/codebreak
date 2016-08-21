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
    public sealed class SpellFrame : AbstractNetworkFrame<SpellFrame, CharacterEntity, string>
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
        private void SpellMove(CharacterEntity character, string message)
        {
            var data = message.Substring(2).Split('|');
            if (data.Length != 2)
            {
                character.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                return;
            }

            var spellId = int.Parse(data[0]);
            var position = int.Parse(data[1]);

            character.AddMessage(() =>
                {
                    if (character.SpellBook == null)
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    if (!character.SpellBook.HasSpell(spellId))
                    {
                        character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                        return;
                    }

                    character.SpellBook.MoveSpell(spellId, position);
                    character.Dispatch(WorldMessage.BASIC_NO_OPERATION());
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        /// <param name="message"></param>
        private void SpellBoost(CharacterEntity character, string message)
        {
            var spellId = -1;
            if (!int.TryParse(message.Substring(2), out spellId))
            {
                character.SafeDispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                return;
            }

            character.AddMessage(() =>
            {
                if (character.SpellBook == null)
                {
                    character.Dispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                    return;
                }

                if (!character.SpellBook.HasSpell(spellId))
                {
                    character.Dispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                    return;
                }

                var spell = character.SpellBook.GetSpellLevel(spellId);

                if (character.SpellPoint < spell.Level)
                {
                    character.Dispatch(WorldMessage.SPELL_UPGRADE_ERROR());
                    return;
                }

                character.SpellBook.LevelUp(spellId);
                character.SpellPoint -= spell.Level;

                character.CachedBuffer = true;
                character.Dispatch(WorldMessage.SPELL_UPGRADE_SUCCESS(spellId, spell.Level + 1));
                character.Dispatch(WorldMessage.ACCOUNT_STATS(character));
                character.CachedBuffer = false;
            });
        }
    }
}
