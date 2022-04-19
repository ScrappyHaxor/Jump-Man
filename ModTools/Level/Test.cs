using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModTools.Level
{
    public class Test : Scene
    {
        public Test(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadAssets()
        {
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
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
            base.PostStackRender();
        }


    }
}
