using System;
using System.Collections.Generic;
using System.Text;
using JumpMan.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrapBox.Framework;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using ScrapBox.Framework.Shapes;

namespace JumpMan.Level
{
    public class MainMenu : Scene
    {
        public readonly string[] LevelPool =
        {
            "level1",
            "level2",
        };


        private MainMenuOverlay menuOverlay;
        private SingleplayerOverlay singleplayerOverlay;
        private SettingsOverlay settingsOverlay;

        private SoundOverlay soundSection;
        private ControlsOverlay controlsSection;

        public MainMenu(ScrapApp app)
            : base(app)
        {
            
        }

        public override void Initialize()
        {
            //Edit things like window size here
            base.Initialize();
            
            Parent.EnqueueChange(() =>
            {
                Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 100;
                Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 100;
                Graphics.ApplyChanges();
            });

            Stack.InsertAt(3, new Layer("Super UI"));
            Stack.InsertAt(4, new Layer("Options Section"));
        }

        public override void LoadAssets()
        {
            //Load assets using AssetManager

            AssetManager.LoadResourceFile("menu", Parent.Content);
            //AssetManager.LoadResourceFile("assets", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            //Instance level things
            Renderer.ClearColor = new Color(10, 10, 10);

            soundSection = new SoundOverlay(new ScrapVector(0, 120), new ScrapVector(800, 400));
            controlsSection = new ControlsOverlay(new ScrapVector(0, 120), new ScrapVector(800, 400));

            singleplayerOverlay = new SingleplayerOverlay(LevelPool);
            settingsOverlay = new SettingsOverlay(ScrapVector.Zero, new ScrapVector(800, 600), soundSection, controlsSection);

            menuOverlay = new MainMenuOverlay(singleplayerOverlay, settingsOverlay);
            menuOverlay.Awake();

            base.Load(args);
        }

        public override void Unload()
        {
            //Destroy everything instanced in Load
            base.Unload();
        }

        public override void UnloadAssets()
        {
            //You dont really need to do anything here, my framework unloads most things automatically
            //The exception would be if you didnt use the AssetManager then assets would be unloaded here.
            base.UnloadAssets();
        }

        public override void PreStackTick(double dt)
        {
            //Works like the standard monogame update, is called before any components
            base.PreStackTick(dt);
        }

        public override void PostStackTick(double dt)
        {
            base.PostStackTick(dt);
        }

        public override void PreStackRender()
        {
            //My framework will automatically draw sprites and other components. This method is mainly just for HUD.
            base.PreStackRender();
        }

        public override void PostStackRender()
        {
            //Renderer.RenderPolygonOutline(ellipse.Verticies, Color.Green, MainCamera, null, 5);
            base.PostStackRender();
        }
    }
}
