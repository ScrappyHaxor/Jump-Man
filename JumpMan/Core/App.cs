using System;
using System.Collections.Generic;
using System.Text;

using ScrapBox.Framework;
using ScrapBox.Framework.Managers;

using JumpMan.Level;
using JumpMan.ECS.Systems;

namespace JumpMan.Core
{
    public class App : ScrapApp
    {
        public const string AssemblyName = "Jump Man";

        private readonly MainMenu mainMenu;
        private readonly LevelSelect levelSelect;
        private readonly SingleplayerLevel singleLevel;
        private readonly MultiplayerLevel multiLevel;

        public App()
        {
            mainMenu = new MainMenu(this);
            levelSelect = new LevelSelect(this);
            singleLevel = new SingleplayerLevel(this);
            multiLevel = new MultiplayerLevel(this);

            //The world manager is the master of everything. It handles entities, components, levels, systems. Everything you could think of.
            SceneManager.RegisterScene("Main Menu", mainMenu);
            SceneManager.RegisterScene("Level Select", levelSelect);
            SceneManager.RegisterScene("Developer Level", singleLevel);
            SceneManager.RegisterScene("Multiplayer Level", multiLevel);

            //SceneManager.LoadingScene = new LoadingScreen(this);
        }

        protected override void Initialize()
        {
            SceneManager.SwapScene("Main Menu");
            base.Initialize();
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();
        }

        protected override void Exit(object o, EventArgs e)
        {
            base.Exit(o, e);
        }
    }
}
