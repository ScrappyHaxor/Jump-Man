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

namespace ModTools.Level
{
    public class Test : Scene
    {
        public const double VerticalOffset = -400;
        public const double InitialVerticalSeparationOffset = 50;
        public const double VerticalSeparationOffset = 40;

        public const double ButtonWidthPadding = 60;
        public const double ButtonHeight = 25;

        private MenuLabel mainLabel;
        private List<MenuButton> buttons;
        private MenuButton backButton;

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
            buttons = new List<MenuButton>();
            if (args.Length == 1 && args[0].GetType() == typeof(string))
            {
                List<ScrapVector> positions = new List<ScrapVector>();
                mainLabel = new MenuLabel(new ScrapVector(0, VerticalOffset), "Select a test position");
                mainLabel.Awake();

                LevelData deserialized = LevelService.DeserializeLevelFromFile(args[0].ToString());
                for (int i = 0; i < deserialized.TestPositions.Count; i++)
                {
                    ScrapVector testPosition = deserialized.TestPositions[i];
                    positions.Add(testPosition);
                    ScrapVector position = new ScrapVector(0, VerticalOffset + InitialVerticalSeparationOffset + VerticalSeparationOffset * (i + 1));

                    MenuButton button = new MenuButton(position, ScrapVector.Zero, $"Test position {i} - x: {testPosition.X} y: {testPosition.Y}");
                    Vector2 textSize = button.Label.Font.MeasureString($"Test position {i} - x: {testPosition.X} y: {testPosition.Y}");
                    button.Transform.Dimensions = new ScrapVector(ButtonWidthPadding + textSize.X, ButtonHeight);
                    button.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(button.Transform.Position, button.Transform.Dimensions);

                    button.Button.Pressed += delegate (object o, EventArgs e)
                    {
                        SceneManager.SwapScene("test level", args[0].ToString(), positions[buttons.IndexOf((MenuButton)((Button)o).Owner)]);
                    };

                    button.Awake();
                    buttons.Add(button);
                }

                ScrapVector backPosition;
                if (buttons.LastOrDefault() != null)
                {
                    backPosition = new ScrapVector(0, buttons.LastOrDefault().Transform.Position.Y + VerticalSeparationOffset);
                }
                else
                {
                    backPosition = new ScrapVector(0, VerticalOffset + InitialVerticalSeparationOffset);
                }

                
                backButton = new MenuButton(backPosition, ScrapVector.Zero, "Back");
                Vector2 size = backButton.Label.Font.MeasureString($"Back");
                backButton.Transform.Dimensions = new ScrapVector(ButtonWidthPadding + size.X, ButtonHeight);
                backButton.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(backButton.Transform.Position, backButton.Transform.Dimensions);

                backButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SceneManager.SwapScene("test");
                };
                backButton.Awake();
            }
            else
            {
                mainLabel = new MenuLabel(new ScrapVector(0, VerticalOffset), "Select a level to test");
                mainLabel.Awake();

                string[] files = Directory.GetFiles("Levels\\");
                for (int i = 0; i < files.Length; i++)
                {
                    string file = files[i];
                    ScrapVector position = new ScrapVector(0, VerticalOffset + InitialVerticalSeparationOffset + VerticalSeparationOffset * (i + 1));

                    MenuButton button = new MenuButton(position, ScrapVector.Zero, Path.GetFileNameWithoutExtension(file));
                    Vector2 textSize = button.Label.Font.MeasureString(Path.GetFileNameWithoutExtension(file));
                    button.Transform.Dimensions = new ScrapVector(ButtonWidthPadding + textSize.X, ButtonHeight);
                    button.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(button.Transform.Position, button.Transform.Dimensions);

                    button.Button.Pressed += delegate (object o, EventArgs e)
                    {
                        SceneManager.SwapScene("test", $"{button.Label.Text}.data");
                    };

                    button.Awake();
                    buttons.Add(button);
                }

                ScrapVector backPosition = new ScrapVector(0, buttons.LastOrDefault().Transform.Position.Y + VerticalSeparationOffset);
                backButton = new MenuButton(backPosition, ScrapVector.Zero, "Back");
                Vector2 size = backButton.Label.Font.MeasureString($"Back");
                backButton.Transform.Dimensions = new ScrapVector(ButtonWidthPadding + size.X, ButtonHeight);
                backButton.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(backButton.Transform.Position, backButton.Transform.Dimensions);

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
