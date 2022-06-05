using Microsoft.Xna.Framework;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.IO;
using JumpMan.ECS.Systems;
using JumpMan.UI;

namespace JumpMan.Level
{
    public class LevelSelect : Scene
    {
        public const double VerticalOffset = -400;
        public const double InitialVerticalSeparationOffset = 50;
        public const double VerticalSeparationOffset = 40;

        public const double ButtonWidthPadding = 60;
        public const double ButtonHeight = 25;

        private List<GenericButton> buttons;

        public LevelSelect(ScrapApp app) : base(app)
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
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            buttons = new List<GenericButton>();
            string[] files = Directory.GetFiles("Levels\\");
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                ScrapVector position = new ScrapVector(0, VerticalOffset + InitialVerticalSeparationOffset + VerticalSeparationOffset * (i + 1));

                GenericButton button = new GenericButton(position, ScrapVector.Zero, Path.GetFileNameWithoutExtension(file));
                Vector2 textSize = button.Label.Font.MeasureString(Path.GetFileNameWithoutExtension(file));
                button.Transform.Dimensions = new ScrapVector(ButtonWidthPadding + textSize.X, ButtonHeight);
                button.Button.Shape = new ScrapBox.Framework.Shapes.Rectangle(button.Transform.Position, button.Transform.Dimensions);

                button.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SceneManager.SwapScene("Developer Level", $"{button.Label.Text}.data");
                };

                button.Awake();
                buttons.Add(button);
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
