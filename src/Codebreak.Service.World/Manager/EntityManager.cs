using System.Collections.Generic;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Action;
using Codebreak.Service.World.Game.Entity;
using Codebreak.Service.World.Frame;
using Codebreak.Service.World.Game.Guild;

namespace Codebreak.Service.World.Manager
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityManager : Singleton<EntityManager>
    {
        private Dictionary<long, MerchantEntity> m_merchantById;
        private Dictionary<long, MerchantEntity> m_merchantByAccount;
        private Dictionary<string, MerchantEntity> m_merchantByName;

        private Dictionary<long, CharacterEntity> m_characterById;
        private Dictionary<long, CharacterEntity> m_characterByAccount;
        private Dictionary<string, CharacterEntity> m_characterByName;

        private Dictionary<long, NonPlayerCharacterEntity> m_npcById;

        private Dictionary<long, TaxCollectorEntity> m_taxCollectorById;

        private long m_onlinePlayers = 0;

        /// <summary>
        /// 
        /// </summary>
        public EntityManager()
        {
            m_merchantById = new Dictionary<long, MerchantEntity>();
            m_merchantByAccount = new Dictionary<long, MerchantEntity>();
            m_merchantByName = new Dictionary<string, MerchantEntity>();

            m_characterById = new Dictionary<long, CharacterEntity>();
            m_characterByAccount = new Dictionary<long, CharacterEntity>();
            m_characterByName = new Dictionary<string, CharacterEntity>();

            m_npcById = new Dictionary<long, NonPlayerCharacterEntity>();

            m_taxCollectorById = new Dictionary<long, TaxCollectorEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="npcDAO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public NonPlayerCharacterEntity CreateNpc(NpcInstanceDAO npcDAO)
        {
            var npc = new NonPlayerCharacterEntity(npcDAO, npcDAO.Id);
            npc.StartAction(GameActionTypeEnum.MAP);
            m_npcById.Add(npc.Id, npc);
            return npc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="taxCollectorDAO"></param>
        /// <returns></returns>
        public TaxCollectorEntity CreateTaxCollector(GuildInstance guild, TaxCollectorDAO taxCollectorDAO)
        {
            var taxCollector = new TaxCollectorEntity(guild, taxCollectorDAO);
            taxCollector.StartAction(GameActionTypeEnum.MAP);
            m_taxCollectorById.Add(taxCollector.Id, taxCollector);
            return taxCollector;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxCollector"></param>
        public void RemoveTaxCollector(TaxCollectorEntity taxCollector)
        {
            m_taxCollectorById.Remove(taxCollector.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterDAO"></param>
        /// <returns></returns>
        public CharacterEntity CreateCharacter(int power, CharacterDAO characterDAO)
        {
            var merchant = GetMerchantById(characterDAO.Id);
            if(merchant != null)
            {
                RemoveMerchant(merchant);
                merchant.AddMessage(() =>
                    {
                        foreach(var buyer in merchant.Buyers)
                            buyer.AddMessage(() => buyer.AbortAction(GameActionTypeEnum.EXCHANGE));                        
                        merchant.StopAction(GameActionTypeEnum.MAP);
                        merchant.Dispose();
                    });
            }
            var character = new CharacterEntity(power, characterDAO);         
            m_characterById.Add(character.Id, character);
            m_characterByName.Add(character.Name.ToLower(), character);
            m_characterByAccount.Add(character.AccountId, character);
            m_onlinePlayers++;
            Logger.Info("EntityManager online players : " + m_onlinePlayers);            
            return character;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="characterDAO"></param>
        /// <returns></returns>
        public MerchantEntity CreateMerchant(CharacterDAO characterDAO)
        {
            var merchant = new MerchantEntity(characterDAO);
            m_merchantById.Add(merchant.Id, merchant);
            m_merchantByName.Add(merchant.Name.ToLower(), merchant);
            m_merchantByAccount.Add(merchant.AccountId, merchant);
            return merchant;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void CharacterDisconnected(CharacterEntity character)
        {
            if (character.PartyId != -1)
                PartyManager.Instance.PartyLeave(character);
            if (character.PartyInvitedPlayerId != -1 || character.PartyInviterPlayerId != -1)
                BasicFrame.Instance.PartyRefuse(character, "");
            if (character.GuildInvitedPlayerId != -1 || character.GuildInviterPlayerId != -1)
                BasicFrame.Instance.GuildJoinRefuse(character, "");
                
            character.AddMessage(() =>
            {
                if (character.HasGameAction(GameActionTypeEnum.FIGHT))
                {
                    if (character.CurrentAction != null)
                        character.AbortAction(character.CurrentAction.Type);
                    character.AbortAction(GameActionTypeEnum.FIGHT);
                    return;
                }

                if (character.CurrentAction != null)
                    character.AbortAction(character.CurrentAction.Type, character.Id);
                if (character.HasGameAction(GameActionTypeEnum.MAP))
                    character.AbortAction(GameActionTypeEnum.MAP);
                if (character.CharacterGuild != null)
                    character.CharacterGuild.CharacterDisconnected();

                RemoveCharacter(character);

                character.Dispose();

                if (character.MerchantModeOnDisconnect)
                {
                    WorldService.Instance.AddMessage(() =>
                    {
                        var merchant = CreateMerchant(character.DatabaseRecord);
                        merchant.StartAction(GameActionTypeEnum.MAP);
                    });
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchant"></param>
        public void RemoveMerchant(MerchantEntity merchant)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    m_merchantById.Remove(merchant.Id);
                    m_merchantByName.Remove(merchant.Name.ToLower());
                    m_merchantByAccount.Remove(merchant.AccountId);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="character"></param>
        public void RemoveCharacter(CharacterEntity character)
        {
            WorldService.Instance.AddMessage(() =>
                {
                    m_onlinePlayers--;
                    m_characterById.Remove(character.Id);
                    m_characterByName.Remove(character.Name.ToLower());
                    m_characterByAccount.Remove(character.AccountId);
                    Logger.Info("EntityManager online players : " + m_onlinePlayers);
                });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CharacterEntity GetCharacterById(long id)
        {
            if (m_characterById.ContainsKey(id))
                return m_characterById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public CharacterEntity GetCharacterByAccount(long accountId)
        {
            if (m_characterByAccount.ContainsKey(accountId))
                return m_characterByAccount[accountId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CharacterEntity GetCharacterByName(string name)
        {
            name = name.ToLower();
            if (m_characterByName.ContainsKey(name))
                return m_characterByName[name];
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MerchantEntity GetMerchantById(long id)
        {
            if (m_merchantById.ContainsKey(id))
                return m_merchantById[id];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public MerchantEntity GetMerchantByAccount(long accountId)
        {
            if (m_merchantByAccount.ContainsKey(accountId))
                return m_merchantByAccount[accountId];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MerchantEntity GetMerchantByName(string name)
        {
            name = name.ToLower();
            if (m_merchantByName.ContainsKey(name))
                return m_merchantByName[name];
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NonPlayerCharacterEntity GetNpcById(long id)
        {
            if (m_npcById.ContainsKey(id))
                return m_npcById[id];
            return null;
        }      
    }
}
