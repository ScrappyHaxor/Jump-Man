using System;
using System.Collections.Generic;
using System.Text;

using ScrapBox.Framework;
using ScrapBox.Framework.Managers;

using JumpMan.Level;
using LevelEditor.Level;

namespace LevelEditor.Core
{
    public class App : ScrapApp
    {
        private Editor editor;

        public App()
        {
            editor = new Editor(this);

            WorldManager.RegisterScene("editor", editor);
        }

        protected override void Initialize()
        {
            WorldManager.SwapScene("editor");
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
