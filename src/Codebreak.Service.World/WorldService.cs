using Codebreak.Framework.Command;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Configuration.Providers;
using Codebreak.Framework.Database;
using Codebreak.Framework.Generic;
using Codebreak.Framework.Network;
using Codebreak.Framework.Util;
using Codebreak.RPC.Protocol;
using Codebreak.Service.World.Command;
using Codebreak.Service.World.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Frame;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Game.Job;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.RPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Codebreak.Service.World
{
    /// <summary>
    /// 
    /// </summary>
    public class WorldService : AbstractTcpServer<WorldService, WorldClient>
    {  
        /// <summary>
        /// 
        /// </summary>
        public ConfigurationManager ConfigurationManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CommandManager<WorldCommandContext> CommandManager
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MessageDispatcher Dispatcher
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configPath"></param>
        public void Start(string configPath)
        {
            ConfigurationManager = new ConfigurationManager();
            ConfigurationManager.RegisterAttributes();
            ConfigurationManager.Add(new JsonConfigurationProvider(configPath), true);
            ConfigurationManager.Load();

            CommandManager = new CommandManager<WorldCommandContext>();
            CommandManager.RegisterCommands();

            base.AddUpdatable(Dispatcher = new MessageDispatcher());
            base.AddUpdatable(RPCManager.Instance);

            base.AddTimer(WorldConfig.WORLD_SAVE_INTERVAL, SaveWorld);
            base.AddTimer(WorldConfig.WEB_PLAYERS_CONNECTED_UPDATE_INTERVAL, UpdateOnlinePlayers);

            Crypt.GenerateNetworkKey();            
            WorldDbMgr.Instance.Initialize();
            InteractiveObjectManager.Instance.Initialize();
            JobManager.Instance.Initialize();
            ClientManager.Instance.Initialize();
            SpellManager.Instance.Initialize();
            AuctionHouseManager.Instance.Initialize();
            AreaManager.Instance.Initialize();
            NpcManager.Instance.Initialize();
            SpawnManager.Instance.Initialize();
            PaddockManager.Instance.Initialize();
            MapManager.Instance.Initialize();
            GuildManager.Instance.Initialize();
            EntityManager.Instance.Initialize();
            RPCManager.Instance.Initialize();
            
            base.Start(WorldConfig.WORLD_SERVICE_IP, WorldConfig.WORLD_SERVICE_PORT);
        }

        #region Network

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientConnected(WorldClient client)
        {
            AddMessage(() =>
            {
                client.FrameManager.AddFrame(AuthentificationFrame.Instance);
                client.Send(WorldMessage.HELLO_GAME());
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        protected override void OnClientDisconnected(WorldClient client)
        {
            AddMessage(() =>
            {
                if (client.CurrentCharacter != null)
                {
                    EntityManager.Instance.CharacterDisconnected(client.CurrentCharacter);

                    client.Characters = null;
                    client.CurrentCharacter = null;
                }
                ClientManager.Instance.ClientDisconnected(client);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        protected override void OnDataReceived(WorldClient client, byte[] buffer, int offset, int count)
        {
            foreach (var message in client.Receive(buffer, offset, count))
            {
                Logger.Debug("Client : " + message);
                
                if (client.CurrentCharacter != null)
                {
                    if (client.CurrentCharacter.FrameManager != null)
                    {
                        if (!client.CurrentCharacter.FrameManager.ProcessMessage(message))
                        {
                            client.CurrentCharacter.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        }
                    }
                }
                else
                {
                    if (!client.FrameManager.ProcessMessage(message))
                    {
                        client.Send(WorldMessage.BASIC_NO_OPERATION());
                    }
                }
            }
        }
        
        #endregion

        #region World Management

        /// <summary>
        /// 
        /// </summary>
        public void SaveWorld()
        {
            Stopwatch updateTimer = new Stopwatch();
            WorldService.Instance.AddLinkedMessages( 
                () => WorldService.Instance.Dispatcher.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_WORLD_SAVING)),
                () => RPCManager.Instance.UpdateState(GameStateEnum.STARTING),
                updateTimer.Start,
                WorldDbMgr.Instance.UpdateAll,
                updateTimer.Stop,
                () => Logger.Info("WorldService : World update performed in : " + updateTimer.ElapsedMilliseconds + " ms"),
                () => RPCManager.Instance.UpdateState(GameStateEnum.ONLINE),
                () => WorldService.Instance.Dispatcher.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_WORLD_SAVING_FINISHED))
            );
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateOnlinePlayers()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadString(WorldConfig.WEB_PLAYERS_CONNECTED_UPDATE_URL + ClientManager.Instance.Clients.Count());
                }
            }
            catch(Exception ex)
            {
            }
        }

        #endregion

    }
}
