using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using ScrapBox.Framework.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.UI
{
    public class InGameOverlay : EntityCollection
    {
        public double ButtonWidth = 300;
        public double ButtonHeight = 140;

        public const double WIDTH_OFFSET = 20;
        public const double HEIGHT_OFFSET = 30;

        public override List<Entity> Register { get; set; }

        public GenericLabel TitleLabel;

        public GenericButton ResumeButton;
        public GenericButton SettingsButton;
        public GenericButton MenuButton;
        public GenericButton ExitButton;

        public ScrapVector Position;
        public ScrapVector Dimensions;

        public ScrapBox.Framework.Shapes.Rectangle BackRect;

        public SettingsOverlay SettingsOverlay;

        public InGameOverlay(SettingsOverlay settingsOverlay, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            SettingsOverlay = settingsOverlay;

            TitleLabel = new GenericLabel(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight / 2 + HEIGHT_OFFSET), new ScrapVector(ButtonWidth, ButtonHeight), "Game Menu");
            TitleLabel.Label.Font = AssetManager.FetchFont("temporaryBigger");
            Register.Add(TitleLabel);

            ResumeButton = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 1.5d + HEIGHT_OFFSET * 2), new ScrapVector(dimensions.X * 2 - WIDTH_OFFSET * 2, ButtonHeight), "Resume Game");
            ResumeButton.Label.Font = AssetManager.FetchFont("temporaryBig");
            ResumeButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                if (!IsAwake)
                    return;

                Sleep();
            };

            Register.Add(ResumeButton);

            SettingsButton = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 2.5d + HEIGHT_OFFSET * 3), new ScrapVector(dimensions.X * 2 - WIDTH_OFFSET * 2, ButtonHeight), "Settings");
            SettingsButton.Label.Font = AssetManager.FetchFont("temporaryBig");
            SettingsButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                if (!IsAwake && SettingsOverlay.IsAwake)
                    return;

                SettingsOverlay.OverrideFlag = true;
                SettingsOverlay.Awake();

                Sleep();
            };

            Register.Add(SettingsButton);

            MenuButton = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 3.5d + HEIGHT_OFFSET * 4), new ScrapVector(dimensions.X * 2 - WIDTH_OFFSET * 2, ButtonHeight), "Return to Menu");
            MenuButton.Label.Font = AssetManager.FetchFont("temporaryBig");
            MenuButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("Main Menu");
            };

            Register.Add(MenuButton);

            ExitButton = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 4.5d + HEIGHT_OFFSET * 5), new ScrapVector(dimensions.X * 2 - WIDTH_OFFSET * 2, ButtonHeight), "Exit Game");
            ExitButton.Label.Font = AssetManager.FetchFont("temporaryBig");
            ExitButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                Environment.Exit(0);
            };

            Register.Add(ExitButton);

            BackRect = new ScrapBox.Framework.Shapes.Rectangle(position, dimensions);
        }

        public override void PreLayerTick(double dt)
        {
            BackRect.Position = Position;
            BackRect.Dimensions = Dimensions;
            base.PreLayerTick(dt);
        }

        public override void PreLayerRender(Camera mainCamera)
        {
            TriangulationService.Triangulate(BackRect.Verticies, TriangulationMethod.EAR_CLIPPING, out int[] indicies);

            Renderer.RenderPolygon(BackRect.Verticies, indicies, new Color(1, 6, 36), mainCamera);
            Renderer.RenderPolygonOutline(BackRect.Verticies, new Color(93, 139, 244), mainCamera, null, 2);
            base.PreLayerRender(mainCamera);
        }
    }
}
