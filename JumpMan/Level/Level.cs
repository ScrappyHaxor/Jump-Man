using JumpMan.Container;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using JumpMan.Services;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;

using ScrapBox.Framework.Diagnostics;
using ScrapBox.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.ECS;
using JumpMan.UI;

namespace JumpMan.Level
{
    public partial class Level : Scene
    {
        public const double CameraOffset = -800;

        private LevelData levelData;

        private bool editorFlag;
        private string[] editorMeta;
        private SpriteFont devFont;

        private bool testFlag;

        private double topOfScreen;
        private double bottomOfScreen;

        private SettingsOverlay inGameSettingsOverlay;
        private InGameOverlay pauseOverlay;
        private SoundOverlay soundSection;
        private ControlsOverlay controlsSection;

        public Level(ScrapApp app)
            : base(app)
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

            devFont = AssetManager.FetchFont("temporary");
            RenderDiagnostics.Font = AssetManager.FetchFont("temporary");
            PhysicsDiagnostics.Font = AssetManager.FetchFont("temporary");

            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            PhysicsSystem.Gravity = new ScrapVector(0, 14);
          
            MainCamera.Zoom = 0.5;

            soundSection = new SoundOverlay(new ScrapVector(0, 50), new ScrapVector(800, 400));
            controlsSection = new ControlsOverlay(new ScrapVector(0, 50), new ScrapVector(800, 400));

            inGameSettingsOverlay = new SettingsOverlay(ScrapVector.Zero, new ScrapVector(800, 600), soundSection, controlsSection);
            pauseOverlay = new InGameOverlay(inGameSettingsOverlay, ScrapVector.Zero, new ScrapVector(500, 450));

            if (args.Length == 1)
            {
                if (args[0].GetType() == typeof(string))
                {
                    string levelName = args[0].ToString();
                    levelData = LevelService.DeserializeLevelFromFile(levelName);
                    levelData.Player.Controller.SelectedLevel = levelName;
                }
                else if (args[0].GetType() == typeof(string[]))
                {
                    string[] package = (string[])args[0];
                    levelData = LevelService.DeserializeLevelFromData(package);

                    editorFlag = true;
                    editorMeta = package;
                }
            }
            else if (args.Length == 2)
            {
                if (args[0].GetType() == typeof(string) && args[1].GetType() == typeof(ScrapVector))
                {
                    string levelName = args[0].ToString();
                    levelData = LevelService.DeserializeLevelFromFile(levelName);
                    levelData.Player.Transform.Position = (ScrapVector)args[1];

                    testFlag = true;
                }
                else if (args[0].GetType() == typeof(string) && args[1].GetType() == typeof(SaveFile))
                {
                    string levelName = args[0].ToString();
                    SaveFile file = (SaveFile)args[1];
                    levelData = LevelService.DeserializeLevelFromFile(levelName);
                    levelData.Player.Transform.Position = file.Position;
                    levelData.Player.Controller.SelectedLevel = file.LevelName;

                }
            }

            foreach (Platform p in levelData.Platforms)
            {
                p.Awake();
            }

            foreach (Background b in levelData.Backgrounds)
            {
                b.Awake();
            }
            
            foreach (CosmeticDrop drop in levelData.CosmeticDrops)
            {
                drop.Awake();
            }

            levelData.Player.Awake();

            foreach (MovingPlatform movingPlatform in levelData.MovingPlatforms)
            {
                movingPlatform.Awake();
            }

            foreach (Entity t in levelData.Traps)
            {
                t.Awake();
            }

            if (!editorFlag && !testFlag)
            {
                levelData.EndOfLevel.PurgeComponent(levelData.EndOfLevel.Sprite);
            }
            else
            {
                levelData.EndOfLevel.OverrideFlag = true;
                levelData.EndOfLevel.TestFlag = testFlag;
                levelData.EndOfLevel.EditorFlag = editorFlag;
                levelData.EndOfLevel.Container = new object[] { editorMeta };
            }
            levelData.EndOfLevel.Awake();

            topOfScreen =  MainCamera.Position.Y + -(MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));
            bottomOfScreen = MainCamera.Position.Y + (MainCamera.Bounds.Height / (MainCamera.Bounds.Height / Camera.VirtualHeight));

            base.Load(args);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void UnloadAssets()
        {
            base.UnloadAssets();
        }

        public override void PreStackTick(double dt)
        {
            if (InputManager.IsKeyDown(Keys.Escape) && !testFlag && !editorFlag)
            {
                if (pauseOverlay.IsAwake)
                {
                    pauseOverlay.Sleep();
                    levelData.Player.Controller.Awake();
                }
                else
                {
                    pauseOverlay.Awake();
                    levelData.Player.Controller.Sleep();
                }
                
            }

            if (!pauseOverlay.IsAwake && !pauseOverlay.SettingsOverlay.IsAwake && !levelData.Player.Controller.IsAwake)
                levelData.Player.Controller.Awake();

            if (editorFlag && InputManager.IsKeyDown(Keys.F5))
            {
                object[] container = new object[] { editorMeta };
                SceneManager.SwapScene("editor", container);
            }

            if (testFlag && InputManager.IsKeyDown(Keys.M))
            {
                SceneManager.SwapScene("test");
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

            base.PreStackTick(dt);
        }

        public override void PostStackTick(double dt)
        {
            base.PostStackTick(dt);
        }

        public override void PreStackRender()
        {
            base.PreStackRender();
        }

        public override void PostStackRender()
        {
            if (editorFlag)
            {
                Vector2 textDims = devFont.MeasureString("DEVELOPER MODE - F5 TO RETURN");
                Rectangle viewport = Parent.Window.ClientBounds;
                Renderer.RenderText(devFont, "DEVELOPER MODE - F5 TO RETURN", new ScrapVector(viewport.Width / 2 - textDims.X / 2, 0), Color.White);
            }

            if (testFlag)
            {
                Vector2 textDims = devFont.MeasureString("TEST MODE - M TO RETURN");
                Rectangle viewport = Parent.Window.ClientBounds;
                Renderer.RenderText(devFont, "TEST MODE - M TO RETURN", new ScrapVector(viewport.Width / 2 - textDims.X / 2, 0), Color.White);
            }

            base.PostStackRender();
        }
    }
}
