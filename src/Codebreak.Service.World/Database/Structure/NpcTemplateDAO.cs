using System.Collections.Generic;
using Codebreak.Framework.Database;
using Codebreak.Service.World.Database.Repository;
using System;
using System.Linq;

namespace Codebreak.Service.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RewardEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public class ItemEntry
        {
            /// <summary>
            /// 
            /// </summary>
            public int TemplateId
            {
                get;
                private set;
            }

            /// <summary>
            /// 
            /// </summary>
            public long Quantity
            {
                get;
                private set;
            }

            /// <summary>
            /// 
            /// </summary>
            public ItemTemplateDAO Template
            {
                get
                {
                    if (m_template == null)
                        m_template = ItemTemplateRepository.Instance.GetById(TemplateId);
                    return m_template;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            private ItemTemplateDAO m_template;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="stringData"></param>
            public ItemEntry(int templateId, long quantity)
            {
                TemplateId = templateId;
                Quantity = quantity;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public List<ItemEntry> RequiredItems
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long RequiredKamas
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ItemEntry> RewardedItems
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long RewardedKamas
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataString"></param>
        public RewardEntry(string dataString)
        { 
            RequiredItems = new List<ItemEntry>();
            RewardedItems = new List<ItemEntry>();

            var data = dataString.Split(';');
            var requiredData = data[0];
            var rewardedData = data[1];

            foreach(var required in requiredData.Split(','))
            {
                var subData = required.Split(':');
                var type = subData[0];
                var id = int.Parse(subData[1]);
                var quantity = long.Parse(subData[2]);

                switch(type)
                {
                    case "kamas":
                        RequiredKamas = quantity;
                        break;

                    case "item":
                        RequiredItems.Add(new ItemEntry(id, quantity));
                        break;
                }
            }

            foreach (var reward in rewardedData.Split(','))
            {
                var subData = reward.Split(':');
                var type = subData[0];
                var id = int.Parse(subData[1]);
                var quantity = long.Parse(subData[2]);

                switch (type)
                {
                    case "kamas":
                        RewardedKamas = quantity;
                        break;

                    case "item":
                        RewardedItems.Add(new ItemEntry(id, quantity));
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateIds"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public bool Match(Dictionary<int, long> templates, long kamas)
        {
            return RequiredKamas == kamas && templates.All(template => RequiredItems.Any(required => required.TemplateId == template.Key && required.Quantity == template.Value));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("npctemplate")]
    public sealed class NpcTemplateDAO : DataAccessObject<NpcTemplateDAO>
    {
        [Key]
        public int Id
        {
            get;
            set;
        }
        public int BonusValue
        {
            get;
            set;
        }
        public int GfxID
        {
            get;
            set;
        }
        public int ScaleX
        {
            get;
            set;
        }
        public int ScaleY
        {
            get;
            set;
        }
        public int Sex
        {
            get;
            set;
        }
        public int Color1
        {
            get;
            set;
        }
        public int Color2
        {
            get;
            set;
        }
        public int Color3
        {
            get;
            set;
        }

        public string EntityLook
        {
            get;
            set;
        }

        public int ExtraClip
        {
            get;
            set;
        }
        public int CustomArtwork
        {
            get;
            set;
        }
        public string Sell
        {
            get;
            set;
        }
        public string Exchange
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<RewardEntry> m_rewards;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<RewardEntry> GetRewards()
        {
            if(m_rewards == null)
            {
                m_rewards = new List<RewardEntry>();
                if(Exchange != "-1")
                {
                    foreach(var reward in Exchange.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        m_rewards.Add(new RewardEntry(reward));
                    }
                }
            }
            return m_rewards;
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ItemTemplateDAO> m_templatesToSell;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<ItemTemplateDAO> GetShopList()
        {
            if (m_templatesToSell == null)
            {
                m_templatesToSell = new List<ItemTemplateDAO>();
                if (Sell != "" && Sell != "-1")
                {
                    foreach (var templateId in Sell.Split(','))
                    {
                        var template = ItemTemplateRepository.Instance.GetById(int.Parse(templateId));
                        if (template != null)
                            m_templatesToSell.Add(template);
                    }
                }
            }
            return m_templatesToSell;
        }
    }
}
