﻿using JumpMan.Container;
using JumpMan.ECS.Components;
using LevelEditor.Services;
using LevelEditor.Core;
using LevelEditor.Objects;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;
using LevelEditor.UI;
using ScrapBox.Framework.ECS.Systems;
using JumpMan.Objects;
using ScrapBox.Framework.ECS.Components;

namespace LevelEditor.ECS.Components
{
    public enum Placing
    {
        PLATFORMS,
        BACKGROUNDS,
        PLAYER
    }

    public class EditorController : Controller
    {
        public override string Name => "Editor Controller";

        public const double CAMERA_POSITION_INCREMENT = 10;

        public const int UPPER_SIZE_LIMIT = 1000000;

        public const double SNAP_WIDTH = 32;
        public const double SNAP_HEIGHT = 32;

        public EditorGhost EditorGhost;
        public LevelData Data;
        public Camera Camera;
        public SavePopup SavePopup;
        public Placing PlacingState;

        private CollisionSystem foregroundCollision;
        private CollisionSystem backgroundCollision;

        public bool SaveFlag;
        public bool InstructionFlag;

        public EditorController()
        {
            foregroundCollision = SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND).GetSystem<CollisionSystem>();
            backgroundCollision = SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.BACKGROUND).GetSystem<CollisionSystem>();
        }

        public override void Awake()
        {
            if (EditorGhost == null)
            {
                LogService.Log(App.AssemblyName, "EditorController", "Awake", "Editor Ghost is null.", Severity.ERROR);
                return;
            }

            if (Camera == null)
            {
                LogService.Log(App.AssemblyName, "EditorController", "Awake", "Camera is null.", Severity.ERROR);
                return;
            }


            base.Awake();
        }

        public override void Sleep()
        {
            base.Sleep();
        }

        public override void TakeInput()
        {
            if (SaveFlag)
                return;

            ScrapVector cameraPosition = InputManager.GetMouseWorldPosition(Camera);
            EditorGhost.Transform.Position = new ScrapVector(ScrapMath.SnapOnGrid(cameraPosition.X, SNAP_WIDTH), ScrapMath.SnapOnGrid(cameraPosition.Y, SNAP_HEIGHT));

            if (InputManager.IsKeyHeld(Keys.LeftControl) && InputManager.IsKeyDown(Keys.S))
            {
                SaveFlag = true;
                EditorGhost.Sleep();
                SavePopup.Awake();

                SavePopup.SaveButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    SaveFlag = false;
                    EditorGhost.Awake();
                    LevelService.SerializeLevel($"{SavePopup.SaveName.TextBox.Input}.data", Data);
                    SavePopup.Sleep();
                };

                return;
            }

            if (InputManager.IsKeyDown(Keys.F5))
            {
                string[] package = LevelService.PackageData(Data);
                object[] container = { package };
                SceneManager.SwapScene("test level", container);
            }

            if (InputManager.IsKeyDown(Keys.Q))
            {
                PlacingState++;
                if ((int)PlacingState > Enum.GetValues(typeof(Placing)).Length - 1)
                {
                    PlacingState = 0;
                }

                EditorGhost.ChangeToState(PlacingState);
            }

            if (InputManager.IsKeyDown(Keys.E))
            {
                if (EditorGhost.Sprite.Mode == SpriteMode.TILE)
                    EditorGhost.Sprite.Mode = SpriteMode.SCALE;
                else if (EditorGhost.Sprite.Mode == SpriteMode.SCALE)
                    EditorGhost.Sprite.Mode = SpriteMode.TILE;
            }

            if (InputManager.IsKeyDown(Keys.I))
            {
                InstructionFlag = !InstructionFlag;
            }

            if (InputManager.IsButtonDown(ScrapBox.Framework.Input.Button.LEFT_MOUSE_BUTTON))
            {
                EditorGhost.Place(PlacingState);
            }

            if (InputManager.IsButtonDown(ScrapBox.Framework.Input.Button.RIGHT_MOUSE_BUTTON))
            {
                if (PlacingState == Placing.PLATFORMS)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(Platform))
                    {
                        Platform platform = (Platform)result.other;
                        platform.Sleep();

                        Data.Platforms.Remove(platform);
                    }

                }
                else if (PlacingState == Placing.BACKGROUNDS)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(Background))
                    {
                        Background background = (Background)result.other;
                        background.Sleep();

                        Data.Backgrounds.Remove(background);
                    }
                }
                else if (PlacingState == Placing.PLAYER)
                {
                    Data.Player.Sleep();
                    Data.Player = null;
                }
            }

            if (InputManager.IsKeyHeld(Keys.W))
            {
                Camera.Position += new ScrapVector(0, -CAMERA_POSITION_INCREMENT);
            }

            if (InputManager.IsKeyHeld(Keys.S))
            {
                Camera.Position += new ScrapVector(0, CAMERA_POSITION_INCREMENT);
            }

            if (InputManager.IsKeyHeld(Keys.D))
            {
                Camera.Position += new ScrapVector(CAMERA_POSITION_INCREMENT, 0);
            }

            if (InputManager.IsKeyHeld(Keys.A))
            {
                Camera.Position += new ScrapVector(-CAMERA_POSITION_INCREMENT, 0);
            }

            if (InputManager.IsKeyDown(Keys.Up))
            {
                EditorGhost.Transform.Dimensions += new ScrapVector(0, EditorGhost.Sprite.Texture.Height);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.Y, EditorGhost.Sprite.Texture.Height, EditorGhost.Sprite.Texture.Height * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(EditorGhost.Transform.Dimensions.X, clamped);
            }

            if (InputManager.IsKeyDown(Keys.Down))
            {
                EditorGhost.Transform.Dimensions -= new ScrapVector(0, EditorGhost.Sprite.Texture.Height);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.Y, EditorGhost.Sprite.Texture.Height, EditorGhost.Sprite.Texture.Height * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(EditorGhost.Transform.Dimensions.X, clamped);
            }

            if (InputManager.IsKeyDown(Keys.Right))
            {
                EditorGhost.Transform.Dimensions += new ScrapVector(EditorGhost.Sprite.Texture.Width, 0);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.X, EditorGhost.Sprite.Texture.Width, EditorGhost.Sprite.Texture.Width * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(clamped, EditorGhost.Transform.Dimensions.Y);
            }

            if (InputManager.IsKeyDown(Keys.Left))
            {
                EditorGhost.Transform.Dimensions -= new ScrapVector(EditorGhost.Sprite.Texture.Width, 0);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.X, EditorGhost.Sprite.Texture.Width, EditorGhost.Sprite.Texture.Width * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(clamped, EditorGhost.Transform.Dimensions.Y);
            }
        }
    }
}
