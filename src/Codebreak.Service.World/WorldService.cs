using System.Text;
using System.Threading;
using Codebreak.Framework.Command;
using Codebreak.Framework.Configuration;
using Codebreak.Framework.Configuration.Providers;
using Codebreak.Framework.Network;
using Codebreak.Service.World.Command;
using Codebreak.Service.World.Database;
using Codebreak.Service.World.Database.Repository;
using Codebreak.Service.World.Frame;
using Codebreak.Service.World.Game;
using Codebreak.Service.World.Manager;
using Codebreak.Service.World.RPC;
using System;
using Codebreak.Framework.Database;
using System.Diagnostics;
using Codebreak.RPC.Protocol;
using System.Threading.Tasks;
using Codebreak.Framework.Generic;
using Codebreak.Service.World.Network;
using Codebreak.Service.World.Game.Job;
using System.Collections.Generic;
using Codebreak.Framework.Util;

namespace Codebreak.Service.World
{
    /// <summary>
    /// 
    /// </summary>
    public class WorldService : TcpServerBase<WorldService, WorldClient>
    {
        /// <summary>
        /// 
        /// </summary>
        [Configurable("WorldSaveInternal")]
        public static int WorldSaveInternal = 60 * 1000;

        /// <summary>
        /// 
        /// </summary>
        [Configurable("WorldServiceIP")]
        public static string WorldServiceIP = "127.0.0.1";

        /// <summary>
        /// 
        /// </summary>
        [Configurable("WorldServicePort")]
        public static int WorldServicePort = 5555;

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
            base.AddTimer(WorldSaveInternal, SaveWorld);

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
            MapManager.Instance.Initialize();
            GuildManager.Instance.Initialize();
            EntityManager.Instance.Initialize();
            RPCManager.Instance.Initialize();
            
            base.Start(WorldServiceIP, WorldServicePort);
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

        #endregion
    }
}
