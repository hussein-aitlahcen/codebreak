using Codebreak.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Codebreak.App.Website.Models.Worldservice
{
    /// <summary>
    /// 
    /// </summary>    
    [Table("characterinstance")]
    public sealed class Character : DataAccessObject<Character>
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte Breed
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color1
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color2
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Color3
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Skin
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SkinSize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Vitality
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Wisdom
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Strength
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public int Intelligence
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Agility
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Chance
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Ap
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Mp
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Life
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Energy
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SpellPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CaracPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Restriction
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Experience
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long AccountId
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Dead
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public int MaxLevel
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        public int DeathCount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Sex
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Kamas
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SavedMapId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int SavedCellId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Merchant
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int TitleId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string TitleParams
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int EmoteCapacity
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int DeathType
        {
            get;
            set;
        }
    }
}