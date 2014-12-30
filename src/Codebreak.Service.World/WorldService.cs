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

            AddUpdatable(Dispatcher = new MessageDispatcher());
            AddUpdatable(RPCManager.Instance);
            AddTimer(WorldSaveInternal, UpdateWorld);

            WorldDbMgr.Instance.Initialize();
            AccountManager.Instance.Initialize();
            SpellManager.Instance.Initialize();
            AreaManager.Instance.Initialize();
            MapManager.Instance.Initialize();
            NpcManager.Instance.Initialize();
            GuildManager.Instance.Initialize();
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
                AccountManager.Instance.ClientDisconnected(client);
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
                AddMessage(() =>
                {
                    Logger.Debug("Client : " + message);

					Stopwatch sw = Stopwatch.StartNew();

                    if (client.CurrentCharacter != null)
                    {
                        if (!client.CurrentCharacter.FrameManager.ProcessMessage(message))
                        {
                            client.CurrentCharacter.SafeDispatch(WorldMessage.BASIC_NO_OPERATION());
                        }
                    }
                    else
                    {
                        if (!client.FrameManager.ProcessMessage(message))
                        {
                            client.Send(WorldMessage.BASIC_NO_OPERATION());
                        }
                    }      

					Logger.Debug("Message processed in : " + sw.ElapsedMilliseconds);
                });
            }
        }
        
        #endregion

        #region World Management

        /// <summary>
        /// 
        /// </summary>
        public void UpdateWorld()
        {
            Stopwatch updateTimer = new Stopwatch();
            WorldService.Instance.AddLinkedMessages( 
                () => WorldService.Instance.Dispatcher.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_WORLD_SAVING)),
                () => RPCManager.Instance.UpdateState(GameState.STARTING),
                updateTimer.Start,
                GuildRepository.Instance.UpdateAll,
                TaxCollectorRepository.Instance.UpdateAll,
                CharacterRepository.Instance.UpdateAll,
                CharacterAlignmentRepository.Instance.UpdateAll,
                CharacterGuildRepository.Instance.UpdateAll,
                SpellBookEntryRepository.Instance.UpdateAll,
                InventoryItemRepository.Instance.UpdateAll,
                updateTimer.Stop,
                () => Logger.Info("WorldService : World update performed in : " + updateTimer.ElapsedMilliseconds + " ms"),
                () => RPCManager.Instance.UpdateState(GameState.ONLINE),
                () => WorldService.Instance.Dispatcher.Dispatch(WorldMessage.INFORMATION_MESSAGE(InformationTypeEnum.ERROR, InformationEnum.ERROR_WORLD_SAVING_FINISHED))
            );
        }

        #endregion
    }
}
