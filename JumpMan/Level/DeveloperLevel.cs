using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
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
        private Player player;

        private List<Platform> platforms;

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

            bool playerParsed = false;
            string levelPath = string.Empty;
            if (Debugger.IsAttached)
            {
                levelPath = "../../../levels/level1.data";
            }
            else
            {
                levelPath = "levels/level1.data";
            }


            foreach (string data in File.ReadAllLines(levelPath))
            {
                string[] chunks = data.Split(";");

                if (!playerParsed)
                {
                    playerParsed = true;
                    player = new Player(new ScrapVector(int.Parse(chunks[0]), int.Parse(chunks[1])));
                    continue;
                }

                ScrapVector position = new ScrapVector(int.Parse(chunks[1]), int.Parse(chunks[2]));
                ScrapVector dimensions = new ScrapVector(int.Parse(chunks[3]), int.Parse(chunks[4]));

                Platform p = new Platform(position, dimensions);
                p.Sprite.Texture = AssetManager.FetchTexture(chunks[0]);
                p.Awake();
            }

            
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
            RenderDiagnostics.Draw(ScrapVector.Zero);
            PhysicsDiagnostics.Draw(new ScrapVector(0, 100));
            base.Draw();
        }
    }
}
