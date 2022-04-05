using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using JumpMan.Container;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using JumpMan.Services;
using ScrapBox.Framework;
using ScrapBox.Framework.Diagnostics;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.Level
{
    public class DeveloperLevel : Scene
    {
        private LevelData levelData;

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
            AssetManager.LoadResourceFile("assets", Parent.Content);
            AssetManager.AddSimpleTexture("placeholder", 64, 64, Parent.Graphics.GraphicsDevice, Parent.Content);

            RenderDiagnostics.Font = AssetManager.FetchFont("temporary");
            PhysicsDiagnostics.Font = AssetManager.FetchFont("temporary");
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            string levelPath = string.Empty;
            if (Debugger.IsAttached)
            {
                levelPath = "../../../levels/level1.data";
            }
            else
            {
                levelPath = "levels/level1.data";
            }

            levelData = LevelService.DeserializeLevel(levelPath);

            foreach (Platform p in levelData.platforms)
            {
                p.Awake();
            }
            
            levelData.player.Awake();

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
            RenderDiagnostics.Draw(ScrapVector.Zero);
            PhysicsDiagnostics.Draw(new ScrapVector(0, 100));
            base.Draw();
        }
    }
}
