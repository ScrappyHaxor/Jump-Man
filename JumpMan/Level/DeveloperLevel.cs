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

        }

        public override void Initialize()
        {

            base.Initialize();
        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("assets", Parent.Content);

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
