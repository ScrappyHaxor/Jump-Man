using JumpMan.Container;
using JumpMan.ECS.Components;
using ModTools.Services;
using ModTools.Core;
using ModTools.Objects;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;
using ModTools.UI;
using ScrapBox.Framework.ECS.Systems;
using JumpMan.Objects;
using ScrapBox.Framework.ECS.Components;

namespace ModTools.ECS.Components
{
    public enum Placing
    {
        PLATFORMS,
        MOVING_PLATFORMS,
        BACKGROUNDS,
        PLAYER,
        TEST_POSITION,
        GLUE,
        ILLUSION,
        SCROLLING,
        BOUNCE,
        TELEPORT,
        LEVEL_END,
        COSMETIC
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
                SavePopup.Position = SceneManager.CurrentScene.MainCamera.Position;
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

            if (InputManager.IsKeyDown(Keys.M))
            {
                SceneManager.SwapScene("tool menu");
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

            if (InputManager.IsKeyDown(Keys.OemPlus))
            {
                EditorGhost.IncreaseTextureIndex(PlacingState);
            }

            if (InputManager.IsKeyDown(Keys.OemMinus))
            {
                EditorGhost.DecreaseTextureIndex(PlacingState);
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
                else if (PlacingState == Placing.MOVING_PLATFORMS)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(MovingPlatform))
                    {
                        MovingPlatform platform = (MovingPlatform)result.other;
                        platform.Sleep();

                        Data.MovingPlatforms.Remove(platform);
                    }
                }
                else if (PlacingState == Placing.BACKGROUNDS)
                {
                    RayResult result = backgroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
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
                else if (PlacingState == Placing.TEST_POSITION)
                {
                    if (Data.TestPositions.Contains(EditorGhost.Transform.Position))
                        Data.TestPositions.Remove(EditorGhost.Transform.Position);
                }
                else if (PlacingState == Placing.GLUE)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(Glue))
                    {
                        Glue glueTrap = (Glue)result.other;
                        glueTrap.Sleep();

                        Data.Traps.Remove(glueTrap);
                    }
                }
                else if (PlacingState == Placing.ILLUSION)
                {
                    RayResult result = backgroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(Platform))
                    {
                        Platform illusionTrap = (Platform)result.other;
                        illusionTrap.Sleep();

                        Data.Traps.Remove(illusionTrap);
                    }
                }
                else if (PlacingState == Placing.SCROLLING)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(ScrollingPlatform))
                    {
                        ScrollingPlatform scrollingPlatform = (ScrollingPlatform)result.other;
                        scrollingPlatform.Sleep();

                        Data.Traps.Remove(scrollingPlatform);
                    }
                }
                else if (PlacingState == Placing.BOUNCE)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(FeetBouncePlatform))
                    {
                        FeetBouncePlatform bounceTrap = (FeetBouncePlatform)result.other;
                        bounceTrap.Sleep();

                        Data.Traps.Remove(bounceTrap);
                    }
                }
                else if (PlacingState == Placing.TELEPORT)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(TeleportPlatform))
                    {
                        TeleportPlatform teleTrap = (TeleportPlatform)result.other;
                        teleTrap.Sleep();

                        Data.Traps.Remove(teleTrap);
                    }
                }
                else if (PlacingState == Placing.LEVEL_END)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(EndOfLevel))
                    {
                        EndOfLevel endOfLevel = (EndOfLevel)result.other;
                        endOfLevel.Sleep();

                        Data.EndOfLevel = null;
                    }
                }
                else if (PlacingState == Placing.COSMETIC)
                {
                    RayResult result = foregroundCollision.Raycast(new PointRay(EditorGhost.Transform.Position));
                    if (result.hit && result.other.GetType() == typeof(EndOfLevel))
                    {
                        CosmeticDrop drop = (CosmeticDrop)result.other;
                        drop.Sleep();

                        Data.CosmeticDrops.Remove(drop);
                    }
                }
            }

            if (InputManager.IsKeyDown(Keys.Space))
            {
                EditorGhost.Substitute();
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
                if (PlacingState == Placing.PLAYER || PlacingState == Placing.TEST_POSITION)
                    return;

                EditorGhost.Transform.Dimensions += new ScrapVector(0, EditorGhost.Sprite.Texture.Height);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.Y, EditorGhost.Sprite.Texture.Height, EditorGhost.Sprite.Texture.Height * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(EditorGhost.Transform.Dimensions.X, clamped);
            }

            if (InputManager.IsKeyDown(Keys.Down))
            {
                if (PlacingState == Placing.PLAYER || PlacingState == Placing.TEST_POSITION)
                    return;

                EditorGhost.Transform.Dimensions -= new ScrapVector(0, EditorGhost.Sprite.Texture.Height);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.Y, EditorGhost.Sprite.Texture.Height, EditorGhost.Sprite.Texture.Height * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(EditorGhost.Transform.Dimensions.X, clamped);
            }

            if (InputManager.IsKeyDown(Keys.Right))
            {
                if (PlacingState == Placing.PLAYER || PlacingState == Placing.TEST_POSITION)
                    return;

                EditorGhost.Transform.Dimensions += new ScrapVector(EditorGhost.Sprite.Texture.Width, 0);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.X, EditorGhost.Sprite.Texture.Width, EditorGhost.Sprite.Texture.Width * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(clamped, EditorGhost.Transform.Dimensions.Y);
            }

            if (InputManager.IsKeyDown(Keys.Left))
            {
                if (PlacingState == Placing.PLAYER || PlacingState == Placing.TEST_POSITION)
                    return;

                EditorGhost.Transform.Dimensions -= new ScrapVector(EditorGhost.Sprite.Texture.Width, 0);
                double clamped = ScrapMath.Clamp(EditorGhost.Transform.Dimensions.X, EditorGhost.Sprite.Texture.Width, EditorGhost.Sprite.Texture.Width * UPPER_SIZE_LIMIT);
                EditorGhost.Transform.Dimensions = new ScrapVector(clamped, EditorGhost.Transform.Dimensions.Y);
            }
        }
    }
}
