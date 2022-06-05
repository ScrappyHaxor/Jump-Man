using JumpMan.Container;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;

namespace JumpMan.UI
{
    public enum WaitingKeybind
    {
        LEFT,
        RIGHT,
        JUMP,
        CAMERA_LEFT,
        CAMERA_RIGHT,
        CAMERA_UP,
        CAMERA_DOWN,
        CYCLE_LEFT,
        CYCLE_RIGHT
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

        private GenericLabel leftCameraLabel;
        private GenericLabel rightCameraLabel;
        private GenericLabel upCameraLabel;
        private GenericLabel downCameraLabel;

        private GenericLabel cycleLeftLabel;
        private GenericLabel cycleRightLabel;

        private GenericButton leftHotkey;
        private GenericButton rightHotkey;
        private GenericButton jumpHotkey;

        private GenericButton leftCameraHotkey;
        private GenericButton rightCameraHotkey;
        private GenericButton upCameraHotkey;
        private GenericButton downCameraHotkey;

        private GenericButton cycleLeftHotkey;
        private GenericButton cycleRightHotkey;

        private ScrapVector position;
        private ScrapVector dimensions;

        private SettingsData settingsData;

        private bool waitingForKeyPress;
        private WaitingKeybind waitingForWhatKeybind;

        public ControlsOverlay(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(4))
        {
            this.position = position;
            this.dimensions = dimensions;



            leftLabel = new GenericLabel(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y - ButtonHeight), ScrapVector.Zero, "Move Left");
            leftLabel.Layer = layer;
            leftLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(leftLabel);

            leftHotkey = new GenericButton(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + ButtonHeight / 2), new ScrapVector(ButtonWidth, ButtonHeight), "LeftKey");
            leftHotkey.Layer = layer;
            leftHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            leftHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.LEFT;
                leftHotkey.Label.Text = "Press any key...";
            };
            Register.Add(leftHotkey);

            rightLabel = new GenericLabel(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y - ButtonHeight), ScrapVector.Zero, "Move Right");
            rightLabel.Layer = layer;
            rightLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(rightLabel);

            rightHotkey = new GenericButton(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + ButtonHeight / 2), new ScrapVector(ButtonWidth, ButtonHeight), $"RightKey");
            rightHotkey.Layer = layer;
            rightHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            rightHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.RIGHT;
                rightHotkey.Label.Text = "Press any key...";
            };
            Register.Add(rightHotkey);

            jumpLabel = new GenericLabel(new ScrapVector(position.X, position.Y - dimensions.Y - ButtonHeight), ScrapVector.Zero, "Jump");
            jumpLabel.Layer = layer;
            jumpLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(jumpLabel);

            jumpHotkey = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight / 2), new ScrapVector(ButtonWidth, ButtonHeight), $"JumpKey");
            jumpHotkey.Layer = layer;
            jumpHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            jumpHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.JUMP;
                jumpHotkey.Label.Text = "Press any key...";
            };
            Register.Add(jumpHotkey);

            leftCameraLabel = new GenericLabel(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 2.5), ScrapVector.Zero, "Camera Move Left");
            leftCameraLabel.Layer = layer;
            leftCameraLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(leftCameraLabel);

            leftCameraHotkey = new GenericButton(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 4), new ScrapVector(ButtonWidth, ButtonHeight), $"CameraLeft");
            leftCameraHotkey.Layer = layer;
            leftCameraHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            leftCameraHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.CAMERA_LEFT;
                leftCameraHotkey.Label.Text = "Press any key...";
            };
            Register.Add(leftCameraHotkey);

            rightCameraLabel = new GenericLabel(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 2.5), ScrapVector.Zero, "Camera Move Right");
            rightCameraLabel.Layer = layer;
            rightCameraLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(rightCameraLabel);

            rightCameraHotkey = new GenericButton(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 4), new ScrapVector(ButtonWidth, ButtonHeight), $"CameraRight");
            rightCameraHotkey.Layer = layer;
            rightCameraHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            rightCameraHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.CAMERA_RIGHT;
                rightCameraHotkey.Label.Text = "Press any key...";
            };
            Register.Add(rightCameraHotkey);

            upCameraLabel = new GenericLabel(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 2.5), ScrapVector.Zero, "Camera Move Up");
            upCameraLabel.Layer = layer;
            upCameraLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(upCameraLabel);

            upCameraHotkey = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 4), new ScrapVector(ButtonWidth, ButtonHeight), $"CameraUp");
            upCameraHotkey.Layer = layer;
            upCameraHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            upCameraHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.CAMERA_UP;
                upCameraHotkey.Label.Text = "Press any key...";
            };
            Register.Add(upCameraHotkey);

            downCameraLabel = new GenericLabel(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 6.5), ScrapVector.Zero, "Camera Move Down");
            downCameraLabel.Layer = layer;
            downCameraLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(downCameraLabel);

            downCameraHotkey = new GenericButton(new ScrapVector(position.X, position.Y - dimensions.Y + ButtonHeight * 8), new ScrapVector(ButtonWidth, ButtonHeight), $"CameraDown");
            downCameraHotkey.Layer = layer;
            downCameraHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            downCameraHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.CAMERA_DOWN;
                downCameraHotkey.Label.Text = "Press any key...";
            };
            Register.Add(downCameraHotkey);

            cycleLeftLabel = new GenericLabel(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 6.5), ScrapVector.Zero, "Cycle Texture Left");
            cycleLeftLabel.Layer = layer;
            cycleLeftLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(cycleLeftLabel);

            cycleLeftHotkey = new GenericButton(new ScrapVector(position.X - dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 8), new ScrapVector(ButtonWidth, ButtonHeight), $"CycleLeft");
            cycleLeftHotkey.Layer = layer;
            cycleLeftHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            cycleLeftHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.CYCLE_LEFT;
                cycleLeftHotkey.Label.Text = "Press any key...";
            };
            Register.Add(cycleLeftHotkey);

            cycleRightLabel = new GenericLabel(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 6.5), ScrapVector.Zero, "Cycle Texture Right");
            cycleRightLabel.Layer = layer;
            cycleRightLabel.Label.Font = AssetManager.FetchFont("temporaryBig");
            Register.Add(cycleRightLabel);

            cycleRightHotkey = new GenericButton(new ScrapVector(position.X + dimensions.Y, position.Y - dimensions.Y + ButtonHeight * 8), new ScrapVector(ButtonWidth, ButtonHeight), $"CycleRight");
            cycleRightHotkey.Layer = layer;
            cycleRightHotkey.Label.Font = AssetManager.FetchFont("temporaryBig");
            cycleRightHotkey.Button.Pressed += delegate (object o, EventArgs e)
            {
                waitingForKeyPress = true;
                waitingForWhatKeybind = WaitingKeybind.CYCLE_RIGHT;
                cycleRightHotkey.Label.Text = "Press any key...";
            };
            Register.Add(cycleRightHotkey);
        }

        public override void Awake()
        {
            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
            {
                settingsData = new SettingsData();
                settingsData.SaveSettings();
            }


            leftHotkey.Label.Text = $"{settingsData.LeftKey}";
            rightHotkey.Label.Text = $"{settingsData.RightKey}";
            jumpHotkey.Label.Text = $"{settingsData.JumpKey}";
            leftCameraHotkey.Label.Text = $"{settingsData.CameraLeftKey}";
            rightCameraHotkey.Label.Text = $"{settingsData.CameraRightKey}";
            upCameraHotkey.Label.Text = $"{settingsData.CameraUpKey}";
            downCameraHotkey.Label.Text = $"{settingsData.CameraDownKey}";
            cycleLeftHotkey.Label.Text = $"{settingsData.CycleTextureLeft}";
            cycleRightHotkey.Label.Text = $"{settingsData.CycleTextureRight}";

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
                    case WaitingKeybind.CAMERA_LEFT:
                        leftCameraHotkey.Label.Text = $"{key}";
                        settingsData.CameraLeftKey = key;
                        break;
                    case WaitingKeybind.CAMERA_RIGHT:
                        rightCameraHotkey.Label.Text = $"{key}";
                        settingsData.CameraRightKey = key;
                        break;
                    case WaitingKeybind.CAMERA_UP:
                        upCameraHotkey.Label.Text = $"{key}";
                        settingsData.CameraUpKey = key;
                        break;
                    case WaitingKeybind.CAMERA_DOWN:
                        downCameraHotkey.Label.Text = $"{key}";
                        settingsData.CameraDownKey = key;
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
