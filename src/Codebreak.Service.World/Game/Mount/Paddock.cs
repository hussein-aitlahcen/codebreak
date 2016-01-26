using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Database.Structure;
using Codebreak.Service.World.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codebreak.Service.World.Game.Mount
{
    public sealed class Paddock 
    {
        public int MapId
        {
            get { return m_record.MapId; }
        }
        public int GuildId
        {
            get { return m_record.GuildId; }
            set { m_record.GuildId = value; }
        }
        public int CellId
        {
            get { return m_record.CellId; }
        }
        public long DefaultPrice
        {
            get { return m_record.DefaultPrice; }
        }
        public long Price
        {
            get { return m_record.Price; }
            set { m_record.Price = value; }
        }
        public int MountPlace
        {
            get { return m_record.MountPlace; }
        }
        public int ItemPlace
        {
            get { return m_record.ItemPlace; }
        }

        private PaddockDAO m_record;
        
        public Paddock(PaddockDAO record)
        {
            m_record = record;
        }
    }
}
