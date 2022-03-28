using System;
using System.Collections.Generic;
using System.Text;

using ScrapBox.Framework;
using ScrapBox.Framework.Managers;

using JumpMan.Level;

namespace JumpMan.Core
{
    public class App : ScrapApp
    {
        MainMenu mainMenu;
        DeveloperLevel devLevel;

        public App()
        {
            mainMenu = new MainMenu(this);
            devLevel = new DeveloperLevel(this);

            //The world manager is the master of everything. It handles entities, components, levels, systems. Everything you could think of.
            WorldManager.RegisterScene("Main Menu", mainMenu);
            WorldManager.RegisterScene("Developer Level", devLevel);
        }

        protected override void Initialize()
        {
            WorldManager.SwapScene("Main Menu");
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
