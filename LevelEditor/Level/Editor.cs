using JumpMan.Container;
using JumpMan.Objects;
using LevelEditor.Objects;
using LevelEditor.Services;
using LevelEditor.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrapBox.Framework;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;

namespace LevelEditor.Level
{
    public enum Placing
    {
        PLATFORMS,
        PLAYER
    }

    public class Editor : Scene
    {
        public const double DEFAULT_PLATFORM_WIDTH = 64;
        public const double DEFAULT_PLATFORM_HEIGHT = 64;

        public const int PLATFORM_SIZE_UPPER_LIMIT = 1000000;

        public const double PLATFORM_SNAP_WIDTH = 32;
        public const double PLATFORM_SNAP_HEIGHT = 32;

        public const double CAMERA_POSITION_INCREMENT = 10;
        public const string PLAYER_TEXTURE_NAME = "player";

        public List<string> platformTextures = new List<string>()
        {
            "placeholder"
        };

        private LevelData data;

        private Placing placingWhat;

        private double platformX;
        private double platformY;

        private double platformWidth;
        private double platformHeight;

        private int platformTextureIndex;

        private Platform fakePlayer;
        private Platform ghost;

        private Marker marker;

        private CollisionSystem collisionSystem;

        private SpriteFont editorFontBig;
        private SpriteFont editorFontSmall;

        private SavePopup savePopup;

        private bool saving;

        public Editor(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("assets", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            savePopup = new SavePopup(ScrapVector.Zero, new ScrapVector(600, 250));

            editorFontBig = AssetManager.FetchFont("editorBig");
            editorFontSmall = AssetManager.FetchFont("editorSmall");

            MainCamera.Zoom = 0.5;

            collisionSystem = WorldManager.GetSystem<CollisionSystem>();

            platformWidth = DEFAULT_PLATFORM_WIDTH;
            platformHeight = DEFAULT_PLATFORM_HEIGHT;

            fakePlayer = new Platform(PLAYER_TEXTURE_NAME, ScrapVector.Zero, new ScrapVector(platformWidth, platformHeight));

            data = new LevelData();

            if (args.Length != 0 && args[0].GetType() == typeof(string))
            {
                data = JumpMan.Services.LevelService.DeserializeLevel(args[0].ToString());
                fakePlayer.Transform.Position = data.player.Transform.Position;
                fakePlayer.Awake();

                foreach (Platform p in data.platforms)
                {
                    p.Awake();
                }
            }

            marker = new Marker(ScrapVector.Zero, new ScrapVector(PLATFORM_SNAP_WIDTH, PLATFORM_SNAP_HEIGHT));
            marker.Awake();

            ghost = new Platform(platformTextures[platformTextureIndex], ScrapVector.Zero, new ScrapVector(platformWidth, platformHeight));

            ScrapVector mousePos = InputManager.GetMouseWorldPosition(MainCamera);
            platformX = ScrapMath.Round(mousePos.X / PLATFORM_SNAP_WIDTH) * PLATFORM_SNAP_WIDTH;
            platformY = ScrapMath.Round(mousePos.Y / PLATFORM_SNAP_HEIGHT) * PLATFORM_SNAP_HEIGHT;
            ghost.Transform.Position = new ScrapVector(platformX, platformY);

            ghost.Awake();
            ghost.Collider.Sleep();

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

            if (placingWhat == Placing.PLATFORMS)
            {
                platformWidth = ScrapMath.Clamp(platformWidth, ghost.Sprite.Texture.Width, ghost.Sprite.Texture.Width * PLATFORM_SIZE_UPPER_LIMIT);
                platformHeight = ScrapMath.Clamp(platformHeight, ghost.Sprite.Texture.Height, ghost.Sprite.Texture.Width * PLATFORM_SIZE_UPPER_LIMIT);
                ghost.Transform.Dimensions = new ScrapVector(platformWidth, platformHeight);
            }

            if (InputManager.IsKeyHeld(Keys.LeftControl) && InputManager.IsKeyDown(Keys.S))
            {
                marker.Sleep();
                savePopup.Awake();
                ghost.Sleep();
                saving = true;

                savePopup.SaveButton.Button.Pressed += delegate (object o, EventArgs e)
                {
                    saving = false;
                    marker.Awake();
                    ghost.Awake();
                    LevelService.SerializeLevel($"{savePopup.SaveName.TextBox.Input}.data", data);
                    savePopup.Sleep();
                };
            }

            if (saving)
                return;

            if (InputManager.IsKeyDown(Keys.Up))
            {
                if (placingWhat == Placing.PLATFORMS)
                    platformHeight += ghost.Sprite.Texture.Height;
            }
            
            if (InputManager.IsKeyDown(Keys.Down))
            {
                if (placingWhat == Placing.PLATFORMS)
                    platformHeight -= ghost.Sprite.Texture.Height;
            }

            if (InputManager.IsKeyDown(Keys.Right))
            {
                if (placingWhat == Placing.PLATFORMS)
                    platformWidth += ghost.Sprite.Texture.Width;
            }

            if (InputManager.IsKeyDown(Keys.Left))
            {
                if (placingWhat == Placing.PLATFORMS)
                    platformWidth -= ghost.Sprite.Texture.Width;
            }

            if (InputManager.IsKeyHeld(Keys.W))
            {
                MainCamera.Transform.Position += new ScrapVector(0, -CAMERA_POSITION_INCREMENT);
            }

            if (InputManager.IsKeyHeld(Keys.S))
            {
                MainCamera.Transform.Position += new ScrapVector(0, CAMERA_POSITION_INCREMENT);
            }

            if (InputManager.IsKeyHeld(Keys.D))
            {
                MainCamera.Transform.Position += new ScrapVector(CAMERA_POSITION_INCREMENT, 0);
            }

            if (InputManager.IsKeyHeld(Keys.A))
            {
                MainCamera.Transform.Position += new ScrapVector(-CAMERA_POSITION_INCREMENT, 0);
            }

            if (InputManager.IsButtonDown(Button.LEFT_MOUSE_BUTTON))
            {
                if (placingWhat == Placing.PLATFORMS)
                {
                    Platform newPlatform = new Platform(platformTextures[platformTextureIndex], ghost.Transform.Position, ghost.Transform.Dimensions);
                    newPlatform.Awake();

                    data.platforms.Add(newPlatform);
                }
                else if (placingWhat == Placing.PLAYER)
                {
                    if (fakePlayer.IsAwake)
                    {
                        fakePlayer.Sleep();
                    }

                    fakePlayer.Transform.Position = ghost.Transform.Position;
                    fakePlayer.Awake();
                }

            }

            if (InputManager.IsButtonDown(Button.RIGHT_MOUSE_BUTTON))
            {
                if (placingWhat == Placing.PLATFORMS || placingWhat == Placing.PLAYER)
                {
                    RayResult result = collisionSystem.Raycast(new PointRay(InputManager.GetMouseWorldPosition(MainCamera)));
                    if (result.hit && result.other.GetType() == typeof(Platform))
                    {
                        Platform p = (Platform)result.other;
                        p.Sleep();
                        
                        if (p != fakePlayer)
                        {
                            data.platforms.Remove(p);
                        }
                    }
                }

            }

            if (InputManager.IsKeyDown(Keys.OemPlus))
            {
                if (placingWhat == Placing.PLATFORMS)
                {
                    platformTextureIndex++;
                    if (platformTextureIndex > platformTextures.Count - 1)
                    {
                        platformTextureIndex = 0;
                    }

                    ghost.Sprite.Texture = AssetManager.FetchTexture(platformTextures[platformTextureIndex]);
                }
            }

            if (InputManager.IsKeyDown(Keys.OemMinus))
            {
                if (placingWhat == Placing.PLATFORMS)
                {
                    platformTextureIndex--;
                    if (platformTextureIndex < 0)
                    {
                        platformTextureIndex = platformTextures.Count - 1;
                    }

                    ghost.Sprite.Texture = AssetManager.FetchTexture(platformTextures[platformTextureIndex]);
                }
            }

            if (InputManager.IsKeyDown(Keys.Q))
            {
                placingWhat++;
                if ((int)placingWhat > Enum.GetValues(typeof(Placing)).Length - 1)
                {
                    placingWhat = 0;
                }

                if (placingWhat == Placing.PLATFORMS)
                {
                    ghost.Sprite.Texture = AssetManager.FetchTexture(platformTextures[platformTextureIndex]);
                    ghost.Transform.Dimensions = new ScrapVector(platformWidth, platformHeight);
                }
                else if (placingWhat == Placing.PLAYER)
                {
                    ghost.Sprite.Texture = AssetManager.FetchTexture(PLAYER_TEXTURE_NAME);
                    ghost.Transform.Dimensions = new ScrapVector(DEFAULT_PLATFORM_WIDTH, DEFAULT_PLATFORM_HEIGHT);
                }
            }

            //savePopup.Update(dt);

            base.Update(dt);
        }

        public override void Draw()
        {
            if (saving)
                return;

            Renderer.RenderLine(ScrapVector.Zero, new ScrapVector(platformX, platformY), Color.White, MainCamera, null, 2);
            Renderer.RenderText(editorFontBig, "Level Editor States", new ScrapVector(10, 10), Color.White);
            Renderer.RenderText(editorFontSmall, $"Placing: {placingWhat}", new ScrapVector(10, 35), Color.White);
            Renderer.RenderText(editorFontSmall, $"Placement Position: {platformX} {platformY}", new ScrapVector(10, 55), Color.White);
            Renderer.RenderText(editorFontSmall, $"Camera Position: {MainCamera.Transform.Position.X} {MainCamera.Transform.Position.Y}", new ScrapVector(10, 75), Color.White);
            

            if (placingWhat == Placing.PLATFORMS)
            {
                Renderer.RenderText(editorFontSmall, $"Platform Size: {platformWidth} {platformHeight}", new ScrapVector(10, 95), Color.White);
                Renderer.RenderText(editorFontSmall, $"Platform Texture: {platformTextures[platformTextureIndex]}", new ScrapVector(10, 115), Color.White);
            }

            //savePopup.Draw(MainCamera);

            //Renderer.RenderGrid(new ScrapVector(-100, -100), new ScrapVector(100, 100), new ScrapVector(PLATFORM_SNAP_WIDTH, PLATFORM_SNAP_HEIGHT), Color.White, MainCamera);
            base.Draw();
        }
    }
}
