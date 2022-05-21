using JumpMan.Container;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.UI
{
    public enum WaitingKeybind
    {
        LEFT,
        RIGHT,
        JUMP
    }

    public class ControlsOverlay : EntityCollection
    {
        public double ButtonWidth = 300;
        public double ButtonHeight = 50;

        public const double HeightOffset = 80;

        public const double WIDTH_OFFSET = 10;
        public const double HEIGHT_OFFSET = 200;

        public const double LEFT_SIDE_OFFSET = 100;

        public override List<Entity> Register { get; set; }

        private GenericLabel leftLabel;
        private GenericLabel rightLabel;
        private GenericLabel jumpLabel;

        private GenericButton leftHotkey;
        private GenericButton rightHotkey;
        private GenericButton jumpHotkey;

        private ScrapVector position;
        private ScrapVector dimensions;

        private SettingsData settingsData;

        private bool waitingForKeyPress;
        private WaitingKeybind waitingForWhatKeybind;

        public ControlsOverlay(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(4))
        {
            this.position = position;
            this.dimensions = dimensions;

            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
                settingsData = new SettingsData();

            settingsData.SaveSettings();

            leftLabel = new GenericLabel(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y - ButtonHeight), ScrapVector.Zero, "Move Left Hotkey");
            leftLabel.Layer = layer;
            leftLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(leftLabel);

            leftHotkey = new GenericButton(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + ButtonHeight / 2), new ScrapVector(ButtonWidth, ButtonHeight), $"{settingsData.LeftKey}");
            leftHotkey.Layer = layer;
            leftHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            leftHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.LEFT;
                leftHotkey.Label.Text = "Press any key...";
            };
            Register.Add(leftHotkey);

            rightLabel = new GenericLabel(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y - ButtonHeight), ScrapVector.Zero, "Move Right Hotkey");
            rightLabel.Layer = layer;
            rightLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(rightLabel);

            rightHotkey = new GenericButton(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + ButtonHeight / 2), new ScrapVector(ButtonWidth, ButtonHeight), $"{settingsData.RightKey}");
            rightHotkey.Layer = layer;
            rightHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            rightHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.RIGHT;
                rightHotkey.Label.Text = "Press any key...";
            };
            Register.Add(rightHotkey);

            jumpLabel = new GenericLabel(new ScrapVector(position.X, position.Y - dimensions.Y - ButtonHeight), ScrapVector.Zero, "Move Jump Hotkey");
            jumpLabel.Layer = layer;
            jumpLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(jumpLabel);

            jumpHotkey = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight / 2), new ScrapVector(ButtonWidth, ButtonHeight), $"{settingsData.JumpKey}");
            jumpHotkey.Layer = layer;
            jumpHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            jumpHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.JUMP;
                jumpHotkey.Label.Text = "Press any key...";
            };
            Register.Add(jumpHotkey);
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Sleep()
        {
            settingsData.SaveSettings();
            base.Sleep();
        }

        public override void PreLayerTick(double dt)
        {
            InputManager.RequestKey((Keys key) =>
            {
                if (!waitingForKeyPress)
                    return;

                switch (waitingForWhatKeybind)
                {
                    case WaitingKeybind.LEFT:
                        leftHotkey.Label.Text = $"{key}";
                        settingsData.LeftKey = key;
                        break;
                    case WaitingKeybind.RIGHT:
                        rightHotkey.Label.Text = $"{key}";
                        settingsData.RightKey = key;
                        break;
                    case WaitingKeybind.JUMP:
                        jumpHotkey.Label.Text = $"{key}";
                        settingsData.JumpKey = key;
                        break;
                }

                waitingForKeyPress = false;
            });

            base.PreLayerTick(dt);
        }

        public override void PreLayerRender(Camera mainCamera)
        {
            base.PreLayerRender(mainCamera);
        }
    }
}
