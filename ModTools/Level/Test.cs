using JumpMan.Container;
using JumpMan.Services;
using Microsoft.Xna.Framework;
using ModTools.UI;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.IO;
using ScrapBox.Framework.ECS.Components;
using System.Text;
using JumpMan.ECS.Systems;
using System.Linq;
using JumpMan.UI;

namespace ModTools.Level
{
    public class Test : Scene
    {
        public const double OffsetY = -500;

        public const double buttonWidth = 850;
        public const double buttonHeight = 100;
        public const double ButtonYOffset = buttonHeight + 40;

        private GenericLabel mainLabel;
        private GenericButton levelButton;
        private List<string> levelFiles;
        private List<ScrapVector> testPositions;
        private GenericButton testButton;
        private GenericButton backButton;

        private int levelIndex;
        private int testIndex;

        public Test(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            ControllerSystem controllerSystem = new ControllerSystem();
            Stack.Fetch(DefaultLayers.FOREGROUND).RegisterSystem(controllerSystem);
        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("menu", Parent.Content);
            AssetManager.LoadResourceFile("editor", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            levelFiles = new List<string>();
            testPositions = new List<ScrapVector>();

            if (args.Length == 1 && args[0].GetType() == typeof(string))
            {
                mainLabel = new GenericLabel(new ScrapVector(0, OffsetY - 60), ScrapVector.Zero, "Select a test position");
                mainLabel.Label.Font = AssetManager.FetchFont("temporaryBiggest");
                mainLabel.Awake();

                LevelData deserialized = LevelService.DeserializeLevelFromFile(args[0].ToString());
                for (int i = 0; i < deserialized.TestPositions.Count; i++)
                {
                    ScrapVector testPosition = deserialized.TestPositions[i];
                    testPositions.Add(testPosition);
                }

                levelButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), new ScrapVector(buttonWidth, buttonHeight), $"{testPositions[testIndex]}");
                levelButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
                levelButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    testIndex++;
                    if (testIndex == testPositions.Count)
                        testIndex = 0;

                    levelButton.Label.Text = $"{testPositions[testIndex]}";
                };
                levelButton.Awake();

                testButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 2), new ScrapVector(buttonWidth, buttonHeight), "Test Level");
                testButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
                testButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SceneManager.SwapScene("test level", args[0].ToString(), testPositions[testIndex]);
                };
                testButton.Awake();

                backButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 3), ScrapVector.Zero, "Back");
                Vector2 size = backButton.Label.Font.MeasureString($"Back");
                backButton.Transform.Dimensions = new ScrapVector(buttonWidth, buttonHeight);
                backButton.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(backButton.Transform.Position, backButton.Transform.Dimensions);
                backButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
                backButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SceneManager.SwapScene("test");
                };
                backButton.Awake();
            }
            else
            {
                mainLabel = new GenericLabel(new ScrapVector(0, OffsetY - 60), ScrapVector.Zero, "Select a level to test");
                mainLabel.Label.Font = AssetManager.FetchFont("temporaryBiggest");
                mainLabel.Awake();

                string[] files = Directory.GetFiles("Levels\\");
                for (int i = 0; i < files.Length; i++)
                {
                    string file = files[i];
                    levelFiles.Add(Path.GetFileNameWithoutExtension(file));
                }

                levelButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), new ScrapVector(buttonWidth, buttonHeight), levelFiles[levelIndex]);
                levelButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
                levelButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    levelIndex++;
                    if (levelIndex == levelFiles.Count)
                        levelIndex = 0;

                    levelButton.Label.Text = levelFiles[levelIndex];
                };
                levelButton.Awake();

                testButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 2), new ScrapVector(buttonWidth, buttonHeight), "Test Level");
                testButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
                testButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SceneManager.SwapScene("test", $"{levelFiles[levelIndex]}.data");
                };
                testButton.Awake();

                ScrapVector backPosition = new ScrapVector(0, OffsetY + ButtonYOffset * 3);
                backButton = new GenericButton(backPosition, ScrapVector.Zero, "Back");
                Vector2 size = backButton.Label.Font.MeasureString($"Back");
                backButton.Transform.Dimensions = new ScrapVector(buttonWidth, buttonHeight);
                backButton.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(backButton.Transform.Position, backButton.Transform.Dimensions);
                backButton.Label.Font = AssetManager.FetchFont("temporaryBigger");

                backButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SceneManager.SwapScene("tool menu");
                };
                backButton.Awake();
            }



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
