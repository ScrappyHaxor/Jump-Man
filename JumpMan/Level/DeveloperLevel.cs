﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using JumpMan.Container;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using JumpMan.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrapBox.Framework;
using ScrapBox.Framework.Diagnostics;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;

namespace JumpMan.Level
{
    public class DeveloperLevel : Scene
    {
        private LevelData levelData;
        private bool developerFlag;
        private string[] developerMeta;
        private SpriteFont devFont;

        public DeveloperLevel(ScrapApp app)
            : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            //Register custom system
            ControllerSystem controllerSystem = new ControllerSystem();
            Stack.Fetch(DefaultLayers.FOREGROUND).RegisterSystem(controllerSystem);
        }

        public override void LoadAssets()
        {
            //Manual loading, remove in the future
            AssetManager.LoadFont("temporary", Parent.Content);
            AssetManager.LoadTexture("player", Parent.Content);
            AssetManager.LoadTexture("placeholder", Parent.Content);
            AssetManager.LoadTexture("placeholder2", Parent.Content);

            //AssetManager.LoadResourceFile("assets", Parent.Content);

            devFont = AssetManager.FetchFont("temporary");
            RenderDiagnostics.Font = AssetManager.FetchFont("temporary");
            PhysicsDiagnostics.Font = AssetManager.FetchFont("temporary");
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            if (args.Length == 1)
            {
                if (args[0].GetType() == typeof(string))
                {
                    string levelName = args[0].ToString();
                    levelData = LevelService.DeserializeLevelFromFile(levelName);
                }
                else if (args[0].GetType() == typeof(string[]))
                {
                    string[] package = (string[])args[0];
                    levelData = LevelService.DeserializeLevelFromData(package);
                    developerFlag = true;
                    developerMeta = package;
                }
            }

            foreach (Platform p in levelData.Platforms)
            {
                p.Awake();
            }

            foreach (Background b in levelData.Backgrounds)
            {
                b.Awake();
            }
            
            levelData.Player.Awake();

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
            if (developerFlag && InputManager.IsKeyDown(Keys.F5))
            {
                object[] container = new object[] { developerMeta };
                SceneManager.SwapScene("editor", container);
            }

            base.PreStackTick(dt);
        }

        public override void PostStackTick(double dt)
        {
            base.PostStackTick(dt);
        }

        public override void PreStackRender()
        {
            if (developerFlag)
            {
                Vector2 textDims = devFont.MeasureString("DEVELOPER MODE - F5 TO RETURN");
                Rectangle viewport = Parent.Window.ClientBounds;
                Renderer.RenderText(devFont, "DEVELOPER MODE - F5 TO RETURN", new ScrapVector(viewport.Width / 2 - textDims.X / 2, 0), Color.White);
            }

            base.PreStackRender();
        }

        public override void PostStackRender()
        {
            base.PostStackRender();
        }
    }
}
