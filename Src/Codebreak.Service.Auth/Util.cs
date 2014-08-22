using System.Text;
using Codebreak.Framework.Generic;
using Codebreak.Framework.Utils;

namespace Codebreak.Service.Auth
{
    public static class Util
    {
        private const int KEY_LENGHT = 32;

        /// <summary>
        /// 
        /// </summary>
        public static char[] HASH = new char[] {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
                't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'X', 'Y', 'Z' , '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'};

        /// <summary>
        /// 
        /// </summary>
        private static FastRandom random = new FastRandom();

        /// <summary>
        /// 
        /// </summary>
        public static ObjectPool<string> AuthKeyPool = new ObjectPool<string>(GenerateLoginKey, AuthService.MAX_CLIENT);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GenerateLoginKey()
        {
            return GenerateString(KEY_LENGHT);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateString(int length)
        {
            var str = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                str.Append(HASH[random.Next(HASH.Length)]);
            }
            return str.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CryptPassword(string key, string password)
        {
            StringBuilder crypted = new StringBuilder();

            for (int i = 0; i < password.Length; i++)
            {
                int pPass = password[i];
                int pKey = key[i];

                int aPass = pPass / 16;
                int aKey = pPass % 16;

                int aNB = (aPass + pKey) % HASH.Length;
                int aNB2 = (aKey + pKey) % HASH.Length;

                crypted.Append(HASH[aNB]);
                crypted.Append(HASH[aNB2]);
            }

            return crypted.ToString();
        }
    }
}
