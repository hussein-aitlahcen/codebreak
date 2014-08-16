using Codebreak.Framework.Database;
using Codebreak.WorldService.World.Database.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.WorldService.World.Database.Structure
{
    /// <summary>
    /// 
    /// </summary>
    [Table("NpcTemplate")]
    public sealed class NpcTemplateDAO : DataAccessObject<NpcTemplateDAO>
    {
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

        private List<ItemTemplateDAO> _templatesToSell;

        public List<ItemTemplateDAO> GetShopList()
        {
            if (_templatesToSell == null)
            {
                _templatesToSell = new List<ItemTemplateDAO>();

                if (Sell != "" && Sell != "-1")
                {
                    foreach (var templateId in Sell.Split(','))
                    {
                        var template = ItemTemplateRepository.Instance.GetTemplate(int.Parse(templateId));
                        if (template != null)
                            _templatesToSell.Add(template);
                    }
                }
            }
            return _templatesToSell;
        }
    }
}
