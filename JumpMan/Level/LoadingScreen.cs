using JumpMan.UI;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.Level
{
    public class LoadingScreen : Scene
    {
        GenericButton test;

        public LoadingScreen(ScrapApp app) : base(app)
        {

        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("game", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            test = new GenericButton(ScrapVector.Zero, new ScrapVector(100, 100), "test");
            base.Load(args);
        }
    }
}
