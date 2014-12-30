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
        private readonly string m_data;

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
            m_data = line;
            Position = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string NextWord()
        {
            return NextWord(" ");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string NextWord(string separator)
        {
            int length = m_data.Length;
            if (Position >= length)
                return null;

            int x;
            if ((x = m_data.IndexOf(separator, Position, StringComparison.Ordinal)) == Position)
            {
                Position += separator.Length;
                return "";
            }

            if (x < 0)
            {
                if (Position == length)
                    return null;
                x = length;
            }

            var word = m_data.Substring(Position, x - Position);

            Position = x + separator.Length;
            if (Position > length)
                Position = length;

            return word;
        }
    }
}
