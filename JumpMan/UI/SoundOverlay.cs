using JumpMan.Container;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.UI
{
    public class SoundOverlay : EntityCollection
    {
        public double SliderWidth = 500;
        public double SliderHeight = 50;

        public const double HeightOffset = 80;

        public const double WIDTH_OFFSET = 10;
        public const double HEIGHT_OFFSET = 200;

        public const double LEFT_SIDE_OFFSET = 100;

        public override List<Entity> Register { get; set; }

        public GenericSlider MusicSlider;
        public GenericSlider EffectSlider;


        private ScrapVector position;
        private ScrapVector dimensions;

        private SettingsData settingsData;

        public SoundOverlay(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(4))
        {
            this.position = position;
            this.dimensions = dimensions;

            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
                settingsData = new SettingsData();

            settingsData.SaveSettings();

            MusicSlider = new GenericSlider(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + SliderHeight / 2), new ScrapVector(SliderWidth, SliderHeight));
            MusicSlider.Layer = layer;
            MusicSlider.Slider.Font = AssetManager.FetchFont("temporaryBig");
            MusicSlider.Slider.UpperBound = 100;
            MusicSlider.Slider.LowerBound = 0;
            MusicSlider.Slider.Label = "Music Volume";
            Register.Add(MusicSlider);

            EffectSlider = new GenericSlider(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + SliderHeight / 2), new ScrapVector(SliderWidth, SliderHeight));
            EffectSlider.Layer = layer;
            EffectSlider.Slider.Font = AssetManager.FetchFont("temporaryBig");
            EffectSlider.Slider.UpperBound = 100;
            EffectSlider.Slider.LowerBound = 0;
            EffectSlider.Slider.Label = "Effect Volume";
            Register.Add(EffectSlider);
        }

        public override void Awake()
        {
            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
                settingsData = new SettingsData();

            settingsData.SaveSettings();

            base.Awake();
            MusicSlider.Slider.SetValue((int)settingsData.MusicVolume);
            EffectSlider.Slider.SetValue((int)settingsData.EffectVolume);
        }

        public override void Sleep()
        {
            settingsData.MusicVolume = MusicSlider.Slider.Value;
            settingsData.EffectVolume = EffectSlider.Slider.Value;
            settingsData.SaveSettings();
            base.Sleep();
        }

        public override void PreLayerRender(Camera mainCamera)
        {
            base.PreLayerRender(mainCamera);
        }
    }
}
