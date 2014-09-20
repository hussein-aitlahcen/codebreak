using System;

namespace Codebreak.Framework.Command
{
    public class TextCommandArgument
    {
        private readonly string _data;

        public int Position
        {
            get; 
            set;
        }

        public TextCommandArgument(string line)
        {
            _data = line;
            Position = 0;
        }

        public string NextWord()
        {
            return NextWord(" ");
        }

        public string NextWord(string separator)
        {
            int length = _data.Length;
            if (Position >= length)
                return null;

            int x;
            if ((x = _data.IndexOf(separator, Position, StringComparison.Ordinal)) == Position)
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

            var word = _data.Substring(Position, x - Position);

            Position = x + separator.Length;
            if (Position > length)
                Position = length;

            return word;
        }
    }
}
