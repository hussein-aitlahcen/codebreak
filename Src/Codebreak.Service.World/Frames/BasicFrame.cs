using System;
using System.Collections.Generic;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Game.Spell;
using Codebreak.Service.World.Manager;

namespace Codebreak.Service.World.Frames
{
    public sealed class BasicFrame : FrameBase<BasicFrame, EntityBase, string>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, EffectEnum> statById = new Dictionary<int, EffectEnum>()
        {
            {10, EffectEnum.AddStrength},
            {11, EffectEnum.AddVitality},
            {12, EffectEnum.AddWisdom},
            {13, EffectEnum.AddChance},
            {14, EffectEnum.AddAgility},
            {15, EffectEnum.AddIntelligence},
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public override Action<EntityBase, string> GetHandler(string message)
        {
            if (message.Length < 2)
                return null;

            switch (message[0])
            {
                case 'A':
                    switch (message[1])
                    {
                        case 'B':
                            //return BoostStats;
                            return null;
                    }
                    break;

                case 'B':
                    switch (message[1])
                    {
                        case 'D':
                            return BasicDate;

                        case 'T':
                            return BasicTime;

                        case 'M':
                            return BasicMessage;
                    }
                    break;

                case 'p':
                    if (message == "ping")
                        return BasicPong;
                    break;

                case 'q':
                    if (message == "qping")
                        return BasicQPong;
                    break;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicPong(EntityBase entity, string message)
        {
            entity.Dispatch(WorldMessage.BASIC_PONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicQPong(EntityBase entity, string message)
        {
            entity.Dispatch(WorldMessage.BASIC_QPONG());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicDate(EntityBase entity, string message)
        {
            entity.Dispatch(WorldMessage.BASIC_DATE());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicTime(EntityBase entity, string message)
        {
            entity.Dispatch(WorldMessage.BASIC_TIME());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="message"></param>
        private void BasicMessage(EntityBase entity, string message)
        {
            var messageData = message.Substring(2).Split('|');
            var channel = messageData[0];
            var messageContent = messageData[1];

            entity.AddMessage(() =>
            {
                if (channel.Length == 1)
                {
                    entity.DispatchChatMessage((ChatChannelEnum)channel[0], messageContent);
                }
                else
                {
                    var remoteEntity = EntityManager.Instance.GetCharacter(channel);

                    if (remoteEntity != null)
                    {
                        entity.DispatchChatMessage(ChatChannelEnum.CHANNEL_PRIVATE_SEND, messageContent, remoteEntity);
                        remoteEntity.DispatchChatMessage(ChatChannelEnum.CHANNEL_PRIVATE_RECEIVE, messageContent, entity);
                    }
                }
            });
        }
    }
}
