using System;
using System.Collections.Generic;
using System.Text;

using ScrapBox.Framework;
using ScrapBox.Framework.Managers;

using JumpMan.Level;
using LevelEditor.Level;
using JumpMan.ECS.Systems;

namespace LevelEditor.Core
{
    public class App : ScrapApp
    {
        public override string AssemblyName { get { return "Jump Man Level Editor"; } }

        private Editor editor;
        private DeveloperLevel devLevel;

        public App()
        {
            editor = new Editor(this);
            devLevel = new DeveloperLevel(this);

            SceneManager.RegisterScene("test level", devLevel);
            SceneManager.RegisterScene("editor", editor);
        }

        protected override void Initialize()
        {
            SceneManager.SwapScene("editor", LaunchArguments);
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
