using JumpMan.Container;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using Microsoft.Xna.Framework;
using ModTools.Objects;
using ModTools.Services;
using ModTools.UI;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.IO;

namespace ModTools.Level
{
    public class Editor : Scene
    {
        public const double AutoInterval = 120000;

        private LevelData data;

        private EditorPlayer editorPlayer;
        private EditorGhost editorGhost;
        private EditorUI editorUI;

        private double topOfScreen;
        private double bottomOfScreen;
        private double leftOfScreen;
        private double rightOfScreen;

        private double lastAuto;

        public Editor(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            ControllerSystem controllerSystem = new ControllerSystem();
            Stack.Fetch(DefaultLayers.FOREGROUND).RegisterSystem(controllerSystem);
        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("editor", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            data = new LevelData();
            if (args.Length == 1)
            {
                if (args[0].GetType() == typeof(string))
                {
                    data = JumpMan.Services.LevelService.DeserializeLevelFromFile(args[0].ToString());
                    data.Player.RigidBody.IsStatic = true;
                    data.Player.PurgeComponent(data.Player.Controller);
                    data.Player.Awake();
                }
                else if (args[0].GetType() == typeof(string[]))
                {
                    string[] package = (string[])args[0];
                    data = JumpMan.Services.LevelService.DeserializeLevelFromData(package);
                    data.Player.RigidBody.IsStatic = true;
                    data.Player.PurgeComponent(data.Player.Controller);
                    data.Player.Awake();
                }
            }

            foreach (Platform p in data.Platforms)
            {
                p.Awake();
            }

            foreach (Background b in data.Backgrounds)
            {
                b.Awake();
            }

            foreach (CosmeticDrop drop in data.CosmeticDrops)
            {
                drop.Awake();
            }

            lastAuto = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            editorGhost = new EditorGhost();
            editorGhost.Data = data;
            editorGhost.Awake();

            editorPlayer = new EditorPlayer();
            editorPlayer.Controller.Data = data;
            editorPlayer.Controller.EditorGhost = editorGhost;
            editorPlayer.Controller.SavePopup = new SavePopup(ScrapVector.Zero, new ScrapVector(600, 250));
            editorPlayer.Awake();

            editorUI = new EditorUI();
            editorUI.Camera = MainCamera;
            editorUI.EditorGhost = editorGhost;
            editorUI.EditorPlayer = editorPlayer;
            editorUI.Awake();

            if (!data.Player.IsAwake)
            {
                data.Player = new Player(ScrapVector.Zero);
                data.Player.RigidBody.IsStatic = true;
                data.Player.PurgeComponent(data.Player.Controller);
                data.Player.Awake();
            }

            foreach (MovingPlatform movingPlatform in data.MovingPlatforms)
            {
                movingPlatform.OverrideFlag = true;
                movingPlatform.Awake();
            }

            foreach (BouncePlatform bouncePlatform in data.BouncePlatforms)
            {
                bouncePlatform.Players.Add(data.Player);
                bouncePlatform.Awake();
            }

            foreach (GluePlatform gluePlatform in data.GluePlatforms)
            {
                gluePlatform.Players.Add(data.Player);
                gluePlatform.Awake();
            }

            foreach (ScrollingPlatform scrollingPlatform in data.ScrollingPlatforms)
            {
                scrollingPlatform.Players.Add(data.Player);
                scrollingPlatform.Awake();
            }

            foreach (TeleportPlatform teleportPlatform in data.TeleportPlatforms)
            {
                teleportPlatform.Players.Add(data.Player);
                teleportPlatform.Awake();
            }

            data.EndOfLevel.PurgeComponent(data.EndOfLevel.Collider);
            data.EndOfLevel.Awake();

            topOfScreen = MainCamera.Position.Y + -(MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
            bottomOfScreen = MainCamera.Position.Y + (MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
            leftOfScreen = MainCamera.Position.X + -(MainCamera.Bounds.Width / (MainCamera.Bounds.Width / Camera.VirtualWidth));
            rightOfScreen = MainCamera.Position.X + (MainCamera.Bounds.Width / (MainCamera.Bounds.Width / Camera.VirtualWidth));

            base.Load(args);
        }

        public override void UnloadAssets()
        {
            base.UnloadAssets();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void PreStackTick(double dt)
        {
            if (editorUI != null && editorPlayer != null)
            {
                editorUI.SaveFlag = editorPlayer.Controller.SaveFlag;
                editorUI.InstructionFlag = editorPlayer.Controller.InstructionFlag;
            }

            base.PreStackTick(dt);
        }

        public override void PostStackTick(double dt)
        {
            if (DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastAuto >= AutoInterval)
            {
                if (!Directory.Exists("autosaves"))
                {
                    Directory.CreateDirectory("autosaves");
                }

                LevelService.SerializeLevel($"autosaves/date_{DateTime.Now:dd-MM-yy}_time_{DateTime.Now:HH-mm-ss}.data", data);
                lastAuto = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }

            base.PostStackTick(dt);
        }

        public override void PreStackRender()
        {
            base.PreStackRender();

        }

        public override void PostStackRender()
        {
            Renderer.RenderLine(new ScrapVector(leftOfScreen, topOfScreen), new ScrapVector(rightOfScreen, topOfScreen), Color.Red, MainCamera, null, 5);
            Renderer.RenderLine(new ScrapVector(leftOfScreen, bottomOfScreen), new ScrapVector(rightOfScreen, bottomOfScreen), Color.Red, MainCamera, null, 5);
            base.PostStackRender();
        }
    }
}
