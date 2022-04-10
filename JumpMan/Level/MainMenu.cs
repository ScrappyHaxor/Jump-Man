using System;
using System.Collections.Generic;
using System.Text;
using JumpMan.UI;
using Microsoft.Xna.Framework;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;

namespace JumpMan.Level
{
    public class MainMenu : Scene
    {
        public const int OffsetY = -200;

        public const int ButtonYSize = 30;
        public const int ButtonYOffset = ButtonYSize + 10;

        private MainMenuButton singleplayer;
        private MainMenuButton multiplayer;
        private MainMenuButton cosmetics;
        private MainMenuButton quit;

        public MainMenu(ScrapApp app)
            : base(app)
        {

        }

        public override void Initialize()
        {
            //Edit things like window size here
            base.Initialize();
        }

        public override void LoadAssets()
        {
            //Load assets using AssetManager

            AssetManager.LoadFont("temporary", Parent.Content);
            //AssetManager.LoadResourceFile("assets", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            //Instance level things

            Renderer.ClearColor = new Color(10, 10, 10);

            ScrapVector buttonDimensions = new ScrapVector(120, ButtonYSize);

            singleplayer = new MainMenuButton(new ScrapVector(0, OffsetY), buttonDimensions, "Singleplayer");
            singleplayer.Button.Pressed += delegate(object sender, EventArgs e)
            {
                WorldManager.SwapScene("Developer Level");
            };

            singleplayer.Awake();

            multiplayer = new MainMenuButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), buttonDimensions, "Multiplayer");
            multiplayer.Button.Pressed += delegate (object sender, EventArgs e)
            {
                WorldManager.SwapScene("Developer Level");
            };

            multiplayer.Awake();

            cosmetics = new MainMenuButton(new ScrapVector(0, OffsetY + ButtonYOffset * 2), buttonDimensions, "Cosmetics");
            cosmetics.Button.Pressed += delegate (object sender, EventArgs e)
            {
                WorldManager.SwapScene("Developer Level");
            };

            cosmetics.Awake();

            quit = new MainMenuButton(new ScrapVector(0, OffsetY + ButtonYOffset * 3), buttonDimensions, "Quit");
            quit.Button.Pressed += delegate (object sender, EventArgs e)
            {
                //0 = Exit code - OK
                Environment.Exit(0);
            };

            quit.Awake();

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

        public override void Update(double dt)
        {
            //Works like the standard monogame update, is called before any components
            base.Update(dt);
        }

        public override void Draw()
        {
            //My framework will automatically draw sprites and other components. This method is mainly just for HUD.
            base.Draw();
        }
    }
}
