using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Codebreak.RPC.Protocol;
using Codebreak.Service.Auth.RPC;

namespace Codebreak.Service.Auth.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthMessage
    {
        private const string CLIENT_VERSION = "1.29.1";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string HELLO_CONNECT(string key)
        {
            return "HC" + key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string PROTOCOL_REQUIRED()
        {
            return "AlEv" + CLIENT_VERSION;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string AUTH_FAILED_CREDENTIALS()
        {
            return "AlEf";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string AUTH_FAILED_BANNED()
        {
            return "AlEb";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string AUTH_FAILED_ALREADY_CONNECTED()
        {
            return "AlEc";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string SERVER_BUSY()
        {
            return "AlEw";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pseudo"></param>
        /// <returns></returns>
        public static string ACCOUNT_PSEUDO(string pseudo)
        {
            return "Ad" + pseudo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string UNKNOW_AC0()
        {
            return "Ac0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="right"></param>
        /// <returns></returns>
        public static string ACCOUNT_RIGHT(int right)
        {
            return "AlK" + right;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string ACCOUNT_SECRET_ANSWER()
        {
            return "AQ";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldServices"></param>
        /// <returns></returns>
        public static string WORLD_HOST_LIST(IEnumerable<AuthRPCServiceClient> worldServices)
        {
            var message = new StringBuilder("AH");
            foreach (var service in worldServices)
            {
                message.Append(service.GameId).Append(';');
                message.Append((int)service.GameState).Append(';'); // Service status ?
                message.Append(0).Append(';'); // Completion
                message.Append(service.GameState == GameState.ONLINE ? '1' : '0'); // CanLog
            }
            return message.ToString();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldServices"></param>
        /// <returns></returns>
        public static string WORLD_CHARACTER_LIST(IEnumerable<AuthRPCServiceClient> worldServices)
        {
            var message = new StringBuilder("AxK31536000000");
            foreach (var service in worldServices)
            {
                message.Append('|');
                message.Append(service.Id).Append(',');
                message.Append("1"); // count
            }
            return message.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string WORLD_SELECTION_FAILED()
        {
            return "AXEd";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public static string WORLD_SELECTION_SUCCESS(string ip, int port, string ticket)
        {
            return "AYK" + ip + ":" + port + ";" + ticket;
        }
    }
}
