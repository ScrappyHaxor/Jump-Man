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
    public class SettingsOverlay : EntityCollection
    {
        public double TitleWidth = 300;
        public double TitleHeight = 50;

        public const double OptionButtonHeight = 80;

        public const double WIDTH_OFFSET = 10;
        public const double HEIGHT_OFFSET = 10;

        public override List<Entity> Register { get; set; }

        public GenericLabel TitleLabel;

        public GenericButton CrossButton;

        public GenericButton Video;
        public GenericButton Sound;
        public GenericButton Gameplay;
        public GenericButton Controls;
        public GenericButton Multiplayer;

        public ScrapVector Position;
        public ScrapVector Dimensions;

        public ScrapBox.Framework.Shapes.Rectangle BackRect;

        public bool OverrideFlag;

        private SoundOverlay soundSection;
        private ControlsOverlay controlsSection;

        public SettingsOverlay(ScrapVector position, ScrapVector dimensions, SoundOverlay soundSection, ControlsOverlay controlsSection) : base(SceneManager.CurrentScene.Stack.Fetch(3))
        {
            this.soundSection = soundSection;
            this.controlsSection = controlsSection;

            TitleLabel = new GenericLabel(new ScrapVector(position.X, position.Y - dimensions.Y + TitleHeight / 2 + 20), new ScrapVector(TitleWidth, TitleHeight), "Options")
            {
                Layer = layer
            };

            TitleLabel.Label.Font = AssetManager.FetchFont("temporaryBigger");
            Register.Add(TitleLabel);

            CrossButton = new GenericButton(new ScrapVector(position.X + dimensions.X - 50, position.Y - dimensions.Y + TitleHeight / 2 + 25), new ScrapVector(80, 80), "X");
            CrossButton.Layer = layer;
            CrossButton.Label.Font = AssetManager.FetchFont("temporaryBig");
            CrossButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                Sleep();

                if (!OverrideFlag)
                {
                    SceneManager.SwapScene("Main Menu");
                }
            };
            Register.Add(CrossButton);

            double ButtonWidth = (dimensions.X * 2) / 5;
            Video = new GenericButton(new ScrapVector(position.X - ButtonWidth * 2, position.Y - dimensions.Y + TitleHeight + OptionButtonHeight / 2 + 50), new ScrapVector(ButtonWidth, OptionButtonHeight), "Video");
            Video.Layer = layer;
            Video.Button.OutlineThickness = 2;
            Video.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(Video);

            Sound = new GenericButton(new ScrapVector(position.X - ButtonWidth * 1, position.Y - dimensions.Y + TitleHeight + OptionButtonHeight / 2 + 50), new ScrapVector(ButtonWidth, OptionButtonHeight), "Sound");
            Sound.Layer = layer;
            Sound.Button.OutlineThickness = 2;
            Sound.Label.Font = AssetManager.FetchFont("temporaryBig");
            Sound.Button.Pressed += delegate (object o, EventArgs e)
            {
                if (controlsSection.IsAwake)
                    controlsSection.Sleep();
                
                if (!soundSection.IsAwake)
                    soundSection.Awake();
            };
            Register.Add(Sound);

            Gameplay = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + TitleHeight + OptionButtonHeight / 2 + 50), new ScrapVector(ButtonWidth, OptionButtonHeight), "Gameplay");
            Gameplay.Layer = layer;
            Gameplay.Button.OutlineThickness = 2;
            Gameplay.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(Gameplay);

            Controls = new GenericButton(new ScrapVector(position.X + ButtonWidth * 1, position.Y - dimensions.Y + TitleHeight + OptionButtonHeight / 2 + 50), new ScrapVector(ButtonWidth, OptionButtonHeight), "Controls");
            Controls.Layer = layer;
            Controls.Button.OutlineThickness = 2;
            Controls.Label.Font = AssetManager.FetchFont("temporaryBig");
            Controls.Button.Pressed += delegate (object o, EventArgs e)
            {
                if (soundSection.IsAwake)
                    soundSection.Sleep();

                if (!controlsSection.IsAwake)
                    controlsSection.Awake();
            };
            Register.Add(Controls);

            Multiplayer = new GenericButton(new ScrapVector(position.X + ButtonWidth * 2, position.Y - dimensions.Y + TitleHeight + OptionButtonHeight / 2 + 50), new ScrapVector(ButtonWidth, OptionButtonHeight), "Multiplayer");
            Multiplayer.Layer = layer;
            Multiplayer.Button.OutlineThickness = 2;
            Multiplayer.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(Multiplayer);

            BackRect = new ScrapBox.Framework.Shapes.Rectangle(position, dimensions);
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Sleep()
        {
            if (soundSection.IsAwake)
                soundSection.Sleep();

            if (controlsSection.IsAwake)
                controlsSection.Sleep();
            base.Sleep();
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
