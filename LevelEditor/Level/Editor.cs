using JumpMan.Objects;
using LevelEditor.Objects;
using Microsoft.Xna.Framework;
using ScrapBox.Framework;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System.Collections.Generic;

namespace LevelEditor.Level
{
    public class Editor : Scene
    {
        public const double DEFAULT_PLATFORM_WIDTH = 10;
        public const double DEFAULT_PLATFORM_HEIGHT = 10;

        public const double PLATFORM_WIDTH_MIN = 10;
        public const double PLATFORM_WIDTH_MAX = 10000;

        public const double PLATFORM_HEIGHT_MIN = 10;
        public const double PLATFORM_HEIGHT_MAX = 10000;

        public const double PLATFORM_SIZE_INCREMENT = 10;

        public const double PLATFORM_SNAP_WIDTH = 5;
        public const double PLATFORM_SNAP_HEIGHT = 5;

        private List<Platform> platforms;

        private double platformX;
        private double platformY;

        private double platformWidth;
        private double platformHeight;

        private Platform ghost;

        private Marker marker;

        public Editor(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadAssets()
        {
            AssetManager.AddSimpleTexture("placeholder", 64, 64, Parent.Graphics.GraphicsDevice, Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            marker = new Marker(ScrapVector.Zero, new ScrapVector(PLATFORM_SNAP_WIDTH, PLATFORM_SNAP_HEIGHT));
            marker.Awake();

            platforms = new List<Platform>();
            ghost = new Platform(ScrapVector.Zero, new ScrapVector(platformWidth, platformHeight));

            platformWidth = DEFAULT_PLATFORM_WIDTH;
            platformHeight = DEFAULT_PLATFORM_HEIGHT;

            ScrapVector mousePos = InputManager.GetMouseWorldPosition(MainCamera);
            platformX = ScrapMath.Round(mousePos.X / PLATFORM_SNAP_WIDTH) * PLATFORM_SNAP_WIDTH;
            platformY = ScrapMath.Round(mousePos.Y / PLATFORM_SNAP_HEIGHT) * PLATFORM_SNAP_HEIGHT;
            ghost.Transform.Position = new ScrapVector(platformX, platformY);

            ghost.Awake();

            base.Load(args);
        }

        public override void UnloadAssets()
        {
            base.UnloadAssets();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void Update(double dt)
        {
            ScrapVector mousePos = InputManager.GetMouseWorldPosition(MainCamera);
            platformX = ScrapMath.Round(mousePos.X / PLATFORM_SNAP_WIDTH) * PLATFORM_SNAP_WIDTH;
            platformY = ScrapMath.Round(mousePos.Y / PLATFORM_SNAP_HEIGHT) * PLATFORM_SNAP_HEIGHT;
            ghost.Transform.Position = new ScrapVector(platformX, platformY);

            platformWidth = ScrapMath.Clamp(platformWidth, PLATFORM_WIDTH_MIN, PLATFORM_WIDTH_MAX);
            platformHeight = ScrapMath.Clamp(platformHeight, PLATFORM_HEIGHT_MIN, PLATFORM_HEIGHT_MAX);
            ghost.Transform.Dimensions = new ScrapVector(platformWidth, platformHeight);

            if (InputManager.IsKeyHeld(Keys.Up))
            {
                platformHeight += PLATFORM_SIZE_INCREMENT;
            }
            
            if (InputManager.IsKeyHeld(Keys.Down))
            {
                platformHeight -= PLATFORM_SIZE_INCREMENT;
            }

            if (InputManager.IsKeyHeld(Keys.Right))
            {
                platformWidth += PLATFORM_SIZE_INCREMENT;
            }

            if (InputManager.IsKeyHeld(Keys.Left))
            {
                platformWidth -= PLATFORM_SIZE_INCREMENT;
            }

            if (InputManager.IsButtonDown(Button.LEFT_MOUSE_BUTTON))
            {
                Platform newPlatform = new Platform(ghost.Transform.Position, ghost.Transform.Dimensions);
                newPlatform.Awake();

                platforms.Add(newPlatform);
            }

            base.Update(dt);
        }

        public override void Draw()
        {
            Renderer.RenderLine(ScrapVector.Zero, new ScrapVector(platformX, platformY), Color.White, MainCamera);
            //Renderer.RenderGrid(new ScrapVector(-100, -100), new ScrapVector(100, 100), new ScrapVector(PLATFORM_SNAP_WIDTH, PLATFORM_SNAP_HEIGHT), Color.White, MainCamera);
            base.Draw();
        }
    }
}
