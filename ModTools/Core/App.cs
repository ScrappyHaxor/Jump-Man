using System;

using ScrapBox.Framework;
using ScrapBox.Framework.Managers;

using JumpMan.Level;
using ModTools.Level;

namespace ModTools.Core
{
    public class App : ScrapApp
    {
        public const string AssemblyName = "Level Editor";

        private readonly Menu toolMenu;
        private readonly Editor editor;
        private readonly Test testPlay;
        private readonly JumpMan.Level.Level devLevel;

        public App()
        {
            toolMenu = new Menu(this);
            editor = new Editor(this);
            testPlay = new Test(this);
            devLevel = new JumpMan.Level.Level(this);

            SceneManager.RegisterScene("tool menu", toolMenu);
            SceneManager.RegisterScene("editor", editor);
            SceneManager.RegisterScene("test", testPlay);
            SceneManager.RegisterScene("test level", devLevel);
        }

        protected override void Initialize()
        {
            if (LaunchArguments.Length == 1)
            {
                SceneManager.SwapScene("editor", LaunchArguments);
            }
            else
            {
                SceneManager.SwapScene("tool menu", LaunchArguments);
            }

            
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
