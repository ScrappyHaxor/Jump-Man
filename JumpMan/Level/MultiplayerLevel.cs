using JumpMan.Container;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using JumpMan.Services;
using JumpMan.UI;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using ScrapBox.Framework;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JumpMan.Level
{
    public class MultiplayerLevel : Scene
    {
        public const int MAX_PLAYERS = 128;
        public const int PORT = 1240;

        private LevelData levelData;

        private bool host;
        private IPEndPoint target;
        private NetServer server;
        private NetClient client;
        private Dictionary<long, Player> players;
        private Dictionary<long, double> jumpLog;
        private Dictionary<long, int> inputLog;

        private bool jumpInitiated;

        private double nextUpdate;

        private double topOfScreen;
        private double bottomOfScreen;

        private SettingsOverlay inGameSettingsOverlay;
        private InGameOverlay pauseOverlay;
        private SoundOverlay soundSection;
        private ControlsOverlay controlsSection;

        public MultiplayerLevel(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            Stack.InsertAt(3, new Layer("Super UI"));
            Stack.InsertAt(4, new Layer("Options Section"));

            //Register custom system
            ControllerSystem controllerSystem = new ControllerSystem();
            Stack.Fetch(DefaultLayers.FOREGROUND).RegisterSystem(controllerSystem);
        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("game", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            if (args.Length == 3)
            {
                if (args[0].GetType() == typeof(string) && args[1].GetType() == typeof(bool) && args[2].GetType() == typeof(string))
                {
                    string levelName = args[0].ToString();
                    levelData = LevelService.DeserializeLevelFromFile(levelName);
                    levelData.Player.Controller.SelectedLevel = levelName;
                    bool.TryParse(args[1].ToString(), out host);
                    if (!IPAddress.TryParse(args[2].ToString(), out IPAddress parsedAddress))
                    {
                        parsedAddress = NetUtility.GetMyAddress(out IPAddress _);
                    }

                    target = new IPEndPoint(parsedAddress, PORT);
                    target.Port = PORT;
                }
            }
            else if (args.Length == 4)
            {
                if (args[0].GetType() == typeof(string) && args[1].GetType() == typeof(SaveFile) && args[2].GetType() == typeof(bool) && args[3].GetType() == typeof(string))
                {
                    string levelName = args[0].ToString();
                    SaveFile file = (SaveFile)args[1];
                    levelData = LevelService.DeserializeLevelFromFile(levelName);
                    levelData.Player.Transform.Position = file.Position;
                    levelData.Player.Controller.SelectedLevel = file.LevelName;
                    bool.TryParse(args[2].ToString(), out host);
                    if (!IPAddress.TryParse(args[3].ToString(), out IPAddress parsedAddress))
                    {
                        parsedAddress = NetUtility.GetMyAddress(out IPAddress _);
                    }

                    target = new IPEndPoint(parsedAddress, PORT);
                    target.Port = PORT;
                }
            }

            if (!host)
            {
                PhysicsSystem.Gravity = ScrapVector.Zero;
            }
            else
            {
                PhysicsSystem.Gravity = new ScrapVector(0, 14);
            }


            nextUpdate = NetTime.Now;

            if (host)
            {
                NetPeerConfiguration serverConfig = new NetPeerConfiguration("jumpman");
                serverConfig.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
                serverConfig.Port = PORT;

                server = new NetServer(serverConfig);
                server.Start();
            }

            players = new Dictionary<long, Player>();
            jumpLog = new Dictionary<long, double>();
            inputLog = new Dictionary<long, int>();

            NetPeerConfiguration clientConfig = new NetPeerConfiguration("jumpman");
            clientConfig.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            client = new NetClient(clientConfig);
            client.Start();

            client.DiscoverKnownPeer(target);

            soundSection = new SoundOverlay(new ScrapVector(0, 50), new ScrapVector(800, 400));
            controlsSection = new ControlsOverlay(new ScrapVector(0, 50), new ScrapVector(800, 400));

            inGameSettingsOverlay = new SettingsOverlay(ScrapVector.Zero, new ScrapVector(800, 600), soundSection, controlsSection);
            pauseOverlay = new InGameOverlay(inGameSettingsOverlay, ScrapVector.Zero, new ScrapVector(500, 450));

            foreach (Platform p in levelData.Platforms)
            {
                p.Awake();
            }

            foreach (MovingPlatform movingPlatform in levelData.MovingPlatforms)
            {
                if (!host)
                    movingPlatform.OverrideFlag = true;

                movingPlatform.Awake();
            }

            foreach (Background b in levelData.Backgrounds)
            {
                b.Awake();
            }

            foreach (BouncePlatform bouncePlatform in levelData.BouncePlatforms)
            {
                bouncePlatform.Awake();
            }

            foreach (GluePlatform gluePlatform in levelData.GluePlatforms)
            {
                gluePlatform.Awake();
            }

            foreach (ScrollingPlatform scrollingPlatform in levelData.ScrollingPlatforms)
            {
                scrollingPlatform.Awake();
            }

            foreach (TeleportPlatform teleportPlatform in levelData.TeleportPlatforms)
            {
                teleportPlatform.Awake();
            }

            levelData.EndOfLevel.PurgeComponent(levelData.EndOfLevel.Sprite);
            levelData.EndOfLevel.Awake();

            base.Load(args);
        }


        public override void PreStackTick(double dt)
        {
            if (host)
            {
                List<Player> copyPlayers = players.Values.ToList();
                foreach (MovingPlatform movingPlatform in levelData.MovingPlatforms)
                {
                    movingPlatform.Players = copyPlayers;
                }

                foreach (BouncePlatform bouncePlatform in levelData.BouncePlatforms)
                {
                    bouncePlatform.Players = copyPlayers;
                }

                foreach (GluePlatform gluePlatform in levelData.GluePlatforms)
                {
                    gluePlatform.Players = copyPlayers;
                }

                foreach (ScrollingPlatform scrollingPlatform in levelData.ScrollingPlatforms)
                {
                    scrollingPlatform.Players = copyPlayers;
                }

                foreach (TeleportPlatform teleportPlatform in levelData.TeleportPlatforms)
                {
                    teleportPlatform.Players = copyPlayers;
                }
            }


            ScrapVector input = ScrapVector.Zero;
            if (InputManager.IsKeyHeld(Keys.A))
            {
                input = new ScrapVector(-1, 0);
            }
            else if (InputManager.IsKeyHeld(Keys.D))
            {
                input = new ScrapVector(1, 0);
            }

            NetOutgoingMessage om;
            if (InputManager.IsKeyHeld(Keys.Space) && !jumpInitiated)
            {
                jumpInitiated = true;
                om = client.CreateMessage();
                om.Write(1);
                client.SendMessage(om, NetDeliveryMethod.Unreliable);
            }

            if (!InputManager.IsKeyHeld(Keys.Space) && jumpInitiated)
            {
                jumpInitiated = false;
                om = client.CreateMessage();
                Console.WriteLine($"input client: {input.X}");
                om.Write(2);
                om.Write((int)input.X);
                client.SendMessage(om, NetDeliveryMethod.Unreliable);
            }

            om = client.CreateMessage();
            om.Write(0);
            om.Write((int)input.X);
            client.SendMessage(om, NetDeliveryMethod.Unreliable);

            if (!jumpInitiated && players.ContainsKey(client.UniqueIdentifier) && players[client.UniqueIdentifier].RigidBody.Grounded())
            {
                Sprite2D sprite = players[client.UniqueIdentifier].Sprite;

                if (input.X < 0)
                    sprite.SourceRectangle = new Rectangle(0, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                else if (input.X > 0)
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                else
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 4, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
            }

            if (host)
            {
                long id;
                NetIncomingMessage serverMessage;
                while ((serverMessage = server.ReadMessage()) != null)
                {
                    switch (serverMessage.MessageType)
                    {
                        case NetIncomingMessageType.DiscoveryRequest:
                            server.SendDiscoveryResponse(null, serverMessage.SenderEndPoint);
                            break;
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus)serverMessage.ReadByte();
                            if (status == NetConnectionStatus.Connected)
                            {
                                id = serverMessage.SenderConnection.RemoteUniqueIdentifier;
                                if (!players.ContainsKey(id))
                                {
                                    players[id] = new Player(new ScrapVector(levelData.Player.Transform.Position.X, levelData.Player.Transform.Position.Y));
                                    players[id].PurgeComponent(players[id].Controller);
                                    players[id].Awake();
                                }
                                LogService.Out($"{NetUtility.ToHexString(serverMessage.SenderConnection.RemoteUniqueIdentifier)} connected....");
                            }
                            else if (status == NetConnectionStatus.Disconnected)
                            {
                                id = serverMessage.SenderConnection.RemoteUniqueIdentifier;
                                if (players.ContainsKey(id))
                                {
                                    players[id].Sleep();
                                    players.Remove(id);
                                }

                                foreach (NetConnection player in server.Connections)
                                {
                                    if (player.RemoteUniqueIdentifier == id)
                                        continue;

                                    NetOutgoingMessage outgoingMessage = server.CreateMessage();
                                    outgoingMessage.Write(id);
                                    outgoingMessage.Write(2);
                                    server.SendMessage(outgoingMessage, player, NetDeliveryMethod.Unreliable);
                                }

                                LogService.Out($"{NetUtility.ToHexString(serverMessage.SenderConnection.RemoteUniqueIdentifier)} disconnected....");
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            id = serverMessage.SenderConnection.RemoteUniqueIdentifier;

                            int type = serverMessage.ReadInt32();

                            if (type == 0)
                            {
                                if (!players[id].RigidBody.Grounded() || jumpLog.ContainsKey(id))
                                    continue;

                                int xInput = serverMessage.ReadInt32();

                                inputLog[id] = xInput;

                                Sprite2D sprite = players[id].Sprite;
                                if (xInput < 0)
                                    sprite.SourceRectangle = new Rectangle(0, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                                else if (xInput > 0)
                                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                                else
                                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 4, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

                                players[id].RigidBody.AddForce(new ScrapVector(xInput * players[id].Controller.MoveForce, 0));
                            }
                            else if (type == 1)
                            {
                                if (!players[id].RigidBody.Grounded() || jumpLog.ContainsKey(id))
                                    continue;

                                jumpLog[id] = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d;
                            }
                            else if (type == 2)
                            {
                                if (!players[id].RigidBody.Grounded() || !jumpLog.ContainsKey(id))
                                    continue;

                                int xInput = serverMessage.ReadInt32();

                                inputLog[id] = xInput;

                                double jumpMultiplier = (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d) - jumpLog[id];
                                jumpMultiplier = ScrapMath.Clamp(jumpMultiplier, players[id].Controller.MinJumpMultiplier, players[id].Controller.MaxJumpMultiplier);

                                ScrapVector jumpForce = new ScrapVector(0, -players[id].Controller.JumpForce * jumpMultiplier);
                                if (xInput != 0)
                                {
                                    Console.WriteLine("2");
                                    Console.WriteLine($"input server: {xInput}");
                                    jumpForce = ScrapMath.RotatePoint(jumpForce, ScrapMath.ToRadians(xInput * players[id].Controller.JumpDirectionalDegree));
                                    Console.WriteLine($"Jump force: {jumpForce}");
                                    players[id].RigidBody.AddForce(jumpForce);
                                }
                                else
                                {
                                    players[id].RigidBody.AddForce(jumpForce);
                                }
                                

                                jumpLog.Remove(id);
                            }
                            break;
                    }
                }

                double now = NetTime.Now;
                if (now > nextUpdate)
                {
                    foreach (NetConnection player in server.Connections)
                    {
                        id = player.RemoteUniqueIdentifier;
                        if (jumpLog.ContainsKey(id) && (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d) - jumpLog[id] >= players[id].Controller.MaxJumpMultiplier / 2 &&
                            inputLog.ContainsKey(id))
                        {
                            Sprite2D sprite = players[id].Sprite;
                            if (inputLog[id] < 0)
                                sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 2, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                            else if (inputLog[id] > 0)
                                sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 3, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                            else
                                sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 5, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                        }

                        foreach (NetConnection otherPlayer in server.Connections)
                        {
                            id = otherPlayer.RemoteUniqueIdentifier;

                            if (!players.ContainsKey(id))
                                continue;

                            Sprite2D sprite = players[id].Sprite;

                            NetOutgoingMessage outgoingMessage = server.CreateMessage();
                            outgoingMessage.Write(id);
                            outgoingMessage.Write(0);
                            outgoingMessage.Write((int)players[id].Transform.Position.X);
                            outgoingMessage.Write((int)players[id].Transform.Position.Y);
                            outgoingMessage.Write(sprite.SourceRectangle.X / sprite.SourceRectangle.Width);
                            outgoingMessage.Write(sprite.SourceRectangle.Y / sprite.SourceRectangle.Height);

                            server.SendMessage(outgoingMessage, player, NetDeliveryMethod.Unreliable);
                        }

                        for (int i = 0; i < levelData.MovingPlatforms.Count; i++)
                        {
                            MovingPlatform platform = levelData.MovingPlatforms[i];
                            id = player.RemoteUniqueIdentifier;

                            NetOutgoingMessage outgoingMessage = server.CreateMessage();
                            outgoingMessage.Write(id);
                            outgoingMessage.Write(1);
                            outgoingMessage.Write(i);
                            outgoingMessage.Write((int)platform.Transform.Position.X);
                            outgoingMessage.Write((int)platform.Transform.Position.Y);

                            server.SendMessage(outgoingMessage, player, NetDeliveryMethod.Unreliable);
                        }
                    }

                    nextUpdate += (1.0 / 60.0);
                }
            }

            NetIncomingMessage clientMessage;
            while ((clientMessage = client.ReadMessage()) != null)
            {
                switch (clientMessage.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        client.Connect(clientMessage.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.Data:
                        if (host)
                            continue;

                        long id = clientMessage.ReadInt64();
                        int type = clientMessage.ReadInt32();

                        if (type == 0)
                        {
                            int x = clientMessage.ReadInt32();
                            int y = clientMessage.ReadInt32();

                            int multiplierX = clientMessage.ReadInt32();
                            int multiplierY = clientMessage.ReadInt32();

                            if (!players.ContainsKey(id))
                            {
                                players[id] = new Player(new ScrapVector(0, 0));
                                players[id].PurgeComponent(players[id].Controller);
                                players[id].Awake();
                            }

                            Sprite2D sprite = players[id].Sprite;
                            sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * multiplierX, sprite.SourceRectangle.Height * multiplierY, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

                            players[id].Transform.Position = new ScrapVector(x, y);
                        }
                        else if (type == 1)
                        {
                            int index = clientMessage.ReadInt32();
                            int x = clientMessage.ReadInt32();
                            int y = clientMessage.ReadInt32();

                            if (index < 0 || index >= levelData.MovingPlatforms.Count)
                                continue;

                            levelData.MovingPlatforms[index].Transform.Position = new ScrapVector(x, y);
                        }
                        else if (type == 2)
                        {
                            players[id].Sleep();
                            players.Remove(id);
                        }
                        break;
                }
            }

            if (levelData.Player.Transform.Position.Y < topOfScreen)
            {
                MainCamera.Position += new ScrapVector(0, -(MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight)) * 2);
                topOfScreen = MainCamera.Position.Y + -(MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
                bottomOfScreen = MainCamera.Position.Y + (MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
            }

            if (levelData.Player.Transform.Position.Y > bottomOfScreen)
            {
                MainCamera.Position += new ScrapVector(0, (MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight)) * 2);
                topOfScreen = MainCamera.Position.Y + -(MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
                bottomOfScreen = MainCamera.Position.Y + (MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
            }

            if (InputManager.IsKeyDown(Keys.Escape))
            {
                if (pauseOverlay.IsAwake)
                {
                    pauseOverlay.Sleep();
                    pauseOverlay.Position = MainCamera.Position;
                    levelData.Player.Controller.Awake();
                }
                else
                {
                    pauseOverlay.Awake();
                    pauseOverlay.Position = MainCamera.Position;
                    levelData.Player.Controller.Sleep();
                }

            }

            if (!pauseOverlay.IsAwake && !pauseOverlay.SettingsOverlay.IsAwake && !levelData.Player.Controller.IsAwake)
                levelData.Player.Controller.Awake();

            base.PreStackTick(dt);
        }

        public override void Unload()
        {
            client.Disconnect("Disconnected by User");
            base.Unload();
        }
    }
}
