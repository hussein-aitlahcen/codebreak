using Codebreak.Framework.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Codebreak.Framework.Util
{
    /// <summary>
    /// 
    /// </summary>
    public static class Crypt
    {
        /// <summary>
        /// 
        /// </summary>
        private static FastRandom Random = new FastRandom();

        /// <summary>
        /// 
        /// </summary>
        private static char[] HEX_CHARS = "0123456789ABCDEF".ToCharArray();

        /// <summary>
        /// 
        /// </summary>
        public static string CRYPT_KEY_PREPARED;

        /// <summary>
        /// 
        /// </summary>
        public static string CRYPT_KEY = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public static int CRYPT_KEY_INDEX = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string PreEscape(string message)
        {
            var result = new StringBuilder();
            for (int i = 0; i < message.Length; i++)
            {
                var current = message[i];
                if (current < 32 || (current > 127 || (current == '%' || current == '+')))
                {
                    result.Append(HttpUtility.UrlEncode("" + current));
                    continue;
                }
                result.Append((char)current);
            }
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static char CheckSum(string message)
        {
            var checkSum = 0;
            for (int i = 0; i < message.Length; i++)
                checkSum += message[i] % 16;
            return HEX_CHARS[checkSum % 16];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string PrepareData(string message)
        {
            var hex = HEX_CHARS[CRYPT_KEY_INDEX];
            var checkSum = CheckSum(message);

            return hex.ToString() + checkSum.ToString() + Cypher(message, CRYPT_KEY_PREPARED, int.Parse(checkSum.ToString(), NumberStyles.HexNumber) * 2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string UnprepareData(string message)
        {
            var checkSum = message[1];
            var decrypted = HttpUtility.UrlDecode(Decypher(message.Substring(2), CRYPT_KEY_PREPARED, int.Parse(checkSum.ToString(), NumberStyles.HexNumber) * 2));
            if (CheckSum(decrypted) != checkSum)
                return message;
            return decrypted;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static void GenerateNetworkKey()
        {
            do
            {
                CRYPT_KEY = InternalGenerateNetworkKey();
            } while ((CRYPT_KEY.Length % 2) != 0 || (CheckSum(CRYPT_KEY.Substring(0, CRYPT_KEY.Length - 1)) != CRYPT_KEY[CRYPT_KEY.Length - 1] ||
                CheckSum(CRYPT_KEY.Substring(1, CRYPT_KEY.Length - 2)) != CRYPT_KEY[0]));

            var prepared = new StringBuilder();
            for (int i = 0; i < CRYPT_KEY.Length; i += 2)
            {
                var current = int.Parse(CRYPT_KEY.Substring(i, 2), NumberStyles.HexNumber);
                prepared.Append((char)current);
            }
            CRYPT_KEY_PREPARED = prepared.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string InternalGenerateNetworkKey()
        {
            var _loc2 = new StringBuilder();
            var _loc3 = 10;
            for (int i = 0; i < _loc3; i++)
                _loc2.Append(GetRandomChar());
            var _loc5 = CheckSum(_loc2.ToString()) + _loc2.ToString();
            return _loc5 + CheckSum(_loc5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRandomChar()
        {
            var _loc2 = Math.Ceiling(Random.NextDouble() * 100);
            if (_loc2 <= 40)
            {
                return ((char)Math.Floor(Random.NextDouble() * 26) + 65).ToString();
            }
            else if (_loc2 <= 80)
            {
                return ((char)Math.Floor(Random.NextDouble() * 26) + 97).ToString();
            }
            else
            {
                return ((char)Math.Floor(Random.NextDouble() * 10) + 48).ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static string Decypher(string message, string key, int serial)
        {
            var result = new StringBuilder();
            var length = key.Length;
            var loc7 = 0;
            for (int i = 0; i < message.Length; i += 2)
            {
                result.Append((char)(int.Parse(message.Substring(i, 2), NumberStyles.HexNumber) ^ key[(loc7++ + serial) % length]));
            }
            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        public static string Cypher(string message, string key, int serial)
        {
            var result = new StringBuilder();
            var length = key.Length;

            message = PreEscape(message);

            for (int i = 0; i < message.Length; i++)
                result.Append(D2h(message[i] ^ key[(i + serial) % length]));


            return result.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string D2h(int value)
        {
            if (value > 255)
                value = 255;
            return HEX_CHARS[(int)Math.Floor(value / 16.0)] + "" + HEX_CHARS[value % 16];
        }
    }
}
