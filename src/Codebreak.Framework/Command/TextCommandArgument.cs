using System;

namespace Codebreak.Framework.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class TextCommandArgument
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string[] m_data;

        /// <summary>
        /// 
        /// </summary>
        public int Position
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public TextCommandArgument(string line)
        {
            m_data = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Position = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string NextWord()
        {
            if (Position >= m_data.Length)
                return string.Empty;
            return m_data[Position++];
        }
    }
}
