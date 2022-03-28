using System;
using System.Collections.Generic;
using System.Text;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.Level
{
    public class DeveloperLevel : Scene
    {
        private Player player;

        //use some way of loading levels from a txt at a later date. For now this will do.
        private Platform platform1;
        private Platform platform2;

        private ControllerSystem controllerSystem;

        public DeveloperLevel(ScrapApp app)
            : base(app)
        {
            //Register custom system in the world manager
            controllerSystem = new ControllerSystem();
            WorldManager.RegisterSystem(controllerSystem);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadAssets()
        {
            //Generating a simple texture for now using my AssetManager
            AssetManager.AddSimpleTexture("placeholder", 64, 64, Parent.Graphics.GraphicsDevice, Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.8;

            platform1 = new Platform(new ScrapVector(0, 64), new ScrapVector(300, 20));
            platform1.Awake();

            platform2 = new Platform(new ScrapVector(250, -100), new ScrapVector(200, 20));
            platform2.Awake();

            player = new Player(ScrapVector.Zero);
            player.Awake();

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

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
