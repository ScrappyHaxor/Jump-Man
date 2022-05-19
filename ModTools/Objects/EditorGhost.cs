using JumpMan.Container;
using JumpMan.Objects;
using Microsoft.Xna.Framework;
using ModTools.Core;
using ModTools.ECS.Components;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModTools.Objects
{
    public class EditorGhost : Entity
    {
        public const string PLAYER_TEXTURE_NAME = "player";
        public const string TEST_TEXTURE_NAME = "placeholder3";
        public const string END_TEXTURE_NAME = "placeholder5";

        public const double MAX_EXTENT = 100000;
        public const double MIN_EXTENT = 100;

        public const double MAX_STEP = 100;
        public const double MIN_STEP = 1;

        public const double MAX_SCROLL_SPEED = 10000;
        public const double MIN_SCROLL_SPEED = 50;

        public const double EXTENT_CHANGE = 10;
        public const double STEP_CHANGE = 1;
        public const double SCROLL_SPEED_CHANGE = 10;

        public List<string> PlatformTextures = new List<string>()
        {
            "placeholder",
            "placeholder4",
            "placeholderTeleport"
        };

        public List<string> MovingPlatformTextures = new List<string>()
        {
            "placeholder",
            "placeholder4"
        };

        public List<string> BackgroundTextures = new List<string>()
        {
            "placeholder2"
        };

        public List<string> TrapTextures = new List<string>()
        {
            "placeholder",
            "placeholder4",
            "placeholderTeleport"
        };

        public int PlatformTextureIndex;
        public int MovingPlatformTextureIndex;
        public int BackgroundTextureIndex;
        public int TrapTextureIndex;

        public double Extent;
        public double Step;
        public bool AxisFlippedFlag;

        public double ScrollSpeed;
        public bool IsLeft;

        public bool GhostPlatformFlag;

        public override string Name => "Editor Ghost";

        public Transform Transform;
        public Sprite2D Sprite;

        public LevelData Data;

        private Placing lastPlacing;

        private CollisionSystem foregroundCollision;
        private CollisionSystem backgroundCollision;

        public EditorGhost() : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            foregroundCollision = SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND).GetSystem<CollisionSystem>();
            backgroundCollision = SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.BACKGROUND).GetSystem<CollisionSystem>();

            Transform = new Transform();
            RegisterComponent(Transform);

            Sprite = new Sprite2D()
            {
                Mode = SpriteMode.TILE
            };

            RegisterComponent(Sprite);
        }

        public void Place(Placing placingState)
        {
            if (placingState == Placing.PLATFORMS)
            {
                Platform platform = new Platform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                if (GhostPlatformFlag)
                {
                    platform.IsGhost = true;
                    platform.PurgeComponent(platform.Sprite);
                }
                platform.Awake();
                platform.Sprite.Mode = Sprite.Mode;

                Data.Platforms.Add(platform);
            }
            else if (placingState == Placing.MOVING_PLATFORMS)
            {
                MovingPlatform platform = new MovingPlatform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                platform.OverrideFlag = true;
                platform.Extent = Extent;
                platform.Step = Step;
                platform.AxisFlippedFlag = AxisFlippedFlag;
                platform.Awake();
                platform.Sprite.Mode = Sprite.Mode;

                Data.MovingPlatforms.Add(platform);
            }
            else if (placingState == Placing.BACKGROUNDS)
            {
                Background background = new Background(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                background.Awake();
                background.Sprite.Mode = Sprite.Mode;

                Data.Backgrounds.Add(background);
            }
            else if (placingState == Placing.PLAYER)
            {
                if (Data.Player != null)
                {
                    Data.Player.Sleep();
                    Data.Player = null;
                }

                Data.Player = new Player(Transform.Position);
                Data.Player.RigidBody.IsStatic = true;
                Data.Player.Awake();
            }
            else if (placingState == Placing.TEST_POSITION)
            {
                Data.TestPositions.Add(Transform.Position);
            }
            else if (placingState == Placing.GLUE)
            {
                Glue trap = new Glue(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.ILLUSION)
            {
                Platform trap = new Platform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Collider.Layer = SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.BACKGROUND);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.SCROLLING)
            {
                ScrollingPlatform trap = new ScrollingPlatform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.ScrollSpeed = ScrollSpeed;
                trap.IsLeft = IsLeft;
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.BOUNCE)
            {
                FeetBouncePlatform trap = new FeetBouncePlatform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.TELEPORT)
            {
                TeleportPlatform trap = new TeleportPlatform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.LEVEL_END)
            {
                if (Data.EndOfLevel != null)
                {
                    Data.EndOfLevel.Sleep();
                    Data.EndOfLevel = null;
                }

                EndOfLevel endOfLevel = new EndOfLevel(Transform.Position, Transform.Dimensions);
                endOfLevel.PurgeComponent(endOfLevel.Collider);
                endOfLevel.Awake();

                Data.EndOfLevel = endOfLevel;
            }
        }

        public void IncreaseTextureIndex(Placing placingState)
        {
            if (placingState == Placing.PLATFORMS)
            {
                PlatformTextureIndex++;
                if (PlatformTextureIndex > PlatformTextures.Count - 1)
                {
                    PlatformTextureIndex = 0;
                }

                Sprite.Texture = AssetManager.FetchTexture(PlatformTextures[PlatformTextureIndex]);
            }
            else if (placingState == Placing.MOVING_PLATFORMS)
            {
                MovingPlatformTextureIndex++;
                if (MovingPlatformTextureIndex > MovingPlatformTextures.Count - 1)
                {
                    MovingPlatformTextureIndex = 0;
                }

                Sprite.Texture = AssetManager.FetchTexture(MovingPlatformTextures[MovingPlatformTextureIndex]);
            }
            else if (placingState == Placing.BACKGROUNDS)
            {
                BackgroundTextureIndex++;
                if (BackgroundTextureIndex > BackgroundTextures.Count - 1)
                {
                    BackgroundTextureIndex = 0;
                }

                Sprite.Texture = AssetManager.FetchTexture(BackgroundTextures[BackgroundTextureIndex]);
            }
            else if (placingState == Placing.GLUE || placingState == Placing.ILLUSION || placingState == Placing.SCROLLING || 
                placingState == Placing.BOUNCE || placingState == Placing.TELEPORT)
            {
                TrapTextureIndex++;
                if (TrapTextureIndex > TrapTextures.Count - 1)
                {
                    TrapTextureIndex = 0;
                }

                Sprite.Texture = AssetManager.FetchTexture(TrapTextures[TrapTextureIndex]);
            }
        }

        public void DecreaseTextureIndex(Placing placingState)
        {
            if (placingState == Placing.PLATFORMS)
            {
                PlatformTextureIndex--;
                if (PlatformTextureIndex < 0)
                {
                    PlatformTextureIndex = PlatformTextures.Count - 1;
                }

                Sprite.Texture = AssetManager.FetchTexture(PlatformTextures[PlatformTextureIndex]);
            }
            else if (placingState == Placing.MOVING_PLATFORMS)
            {
                MovingPlatformTextureIndex--;
                if (MovingPlatformTextureIndex < 0)
                {
                    MovingPlatformTextureIndex = MovingPlatformTextures.Count - 1;
                }

                Sprite.Texture = AssetManager.FetchTexture(MovingPlatformTextures[MovingPlatformTextureIndex]);
            }
            else if (placingState == Placing.BACKGROUNDS)
            {
                BackgroundTextureIndex--;
                if (BackgroundTextureIndex < 0)
                {
                    BackgroundTextureIndex = BackgroundTextures.Count - 1;
                }

                Sprite.Texture = AssetManager.FetchTexture(BackgroundTextures[BackgroundTextureIndex]);
            }
            else if (placingState == Placing.GLUE || placingState == Placing.ILLUSION || placingState == Placing.SCROLLING ||
                placingState == Placing.BOUNCE || placingState == Placing.TELEPORT)
            {
                TrapTextureIndex--;
                if (TrapTextureIndex < 0)
                {
                    TrapTextureIndex = TrapTextures.Count - 1;
                }

                Sprite.Texture = AssetManager.FetchTexture(TrapTextures[TrapTextureIndex]);
            }
        }

        public void ChangeToState(Placing placingState)
        {
            lastPlacing = placingState;

            if (placingState == Placing.PLATFORMS)
            {
                Sprite.Texture = AssetManager.FetchTexture(PlatformTextures[PlatformTextureIndex]);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }
            else if (placingState == Placing.MOVING_PLATFORMS)
            {
                Sprite.Texture = AssetManager.FetchTexture(MovingPlatformTextures[MovingPlatformTextureIndex]);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }
            else if (placingState == Placing.BACKGROUNDS)
            {
                Sprite.Texture = AssetManager.FetchTexture(BackgroundTextures[BackgroundTextureIndex]);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }
            else if (placingState == Placing.PLAYER)
            {
                Sprite.Texture = AssetManager.FetchTexture(PLAYER_TEXTURE_NAME);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }
            else if (placingState == Placing.TEST_POSITION)
            {
                Sprite.Texture = AssetManager.FetchTexture(TEST_TEXTURE_NAME);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }
            else if (placingState == Placing.GLUE || placingState == Placing.ILLUSION || placingState == Placing.SCROLLING ||
                placingState == Placing.BOUNCE || placingState == Placing.TELEPORT)
            {
                Sprite.Texture = AssetManager.FetchTexture(TrapTextures[TrapTextureIndex]);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }
            else if (placingState == Placing.LEVEL_END)
            {
                Sprite.Texture = AssetManager.FetchTexture(END_TEXTURE_NAME);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }

        }

        public void Substitute()
        {
            if (lastPlacing == Placing.PLATFORMS)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(Platform))
                {
                    Platform platform = (Platform)result.other;
                    platform.Sprite.Texture = Sprite.Texture;
                }

            }
            else if (lastPlacing == Placing.MOVING_PLATFORMS)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(MovingPlatform))
                {
                    MovingPlatform platform = (MovingPlatform)result.other;
                    platform.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.BACKGROUNDS)
            {
                RayResult result = backgroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(Background))
                {
                    Background background = (Background)result.other;
                    background.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.GLUE)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(Glue))
                {
                    Glue glueTrap = (Glue)result.other;
                    glueTrap.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.ILLUSION)
            {
                RayResult result = backgroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(Platform))
                {
                    Platform illusionTrap = (Platform)result.other;
                    illusionTrap.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.SCROLLING)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(ScrollingPlatform))
                {
                    ScrollingPlatform scrollingPlatform = (ScrollingPlatform)result.other;
                    scrollingPlatform.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.BOUNCE)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(FeetBouncePlatform))
                {
                    FeetBouncePlatform bounceTrap = (FeetBouncePlatform)result.other;
                    bounceTrap.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.TELEPORT)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(TeleportPlatform))
                {
                    TeleportPlatform teleTrap = (TeleportPlatform)result.other;
                    teleTrap.Sprite.Texture = Sprite.Texture;
                }
            }
            else if (lastPlacing == Placing.LEVEL_END)
            {
                RayResult result = foregroundCollision.Raycast(new PointRay(Transform.Position));
                if (result.hit && result.other.GetType() == typeof(EndOfLevel))
                {
                    EndOfLevel endOfLevel = (EndOfLevel)result.other;
                    endOfLevel.Sprite.Texture = Sprite.Texture;
                }
            }
        }

        public override void Awake()
        {
            if (Data == null)
            {
                LogService.Log(App.AssemblyName, "EditorGhost", "Awake", "Level data is null", Severity.ERROR);
                return;
            }

            if (Sprite.Texture == null)
            {
                Sprite.Texture = AssetManager.FetchTexture(PlatformTextures[PlatformTextureIndex]);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
            }

            base.Awake();
        }

        public override void Sleep()
        {
            base.Sleep();
        }

        public override void PreLayerTick(double dt)
        {
            Extent = ScrapMath.Clamp(Extent, MIN_EXTENT, MAX_EXTENT);
            Step = ScrapMath.Clamp(Step, MIN_STEP, MAX_STEP);
            ScrollSpeed = ScrapMath.Clamp(ScrollSpeed, MIN_SCROLL_SPEED, MAX_SCROLL_SPEED);

            if (InputManager.IsKeyDown(Keys.Delete))
            {
                if (lastPlacing == Placing.MOVING_PLATFORMS)
                    Extent -= EXTENT_CHANGE;
            }
            
            if (InputManager.IsKeyDown(Keys.PageDown))
            {
                if (lastPlacing == Placing.MOVING_PLATFORMS)
                    Extent += EXTENT_CHANGE;
            }

            if (InputManager.IsKeyDown(Keys.Insert))
            {
                if (lastPlacing == Placing.MOVING_PLATFORMS)
                    Step -= STEP_CHANGE;
                else if (lastPlacing == Placing.SCROLLING)
                    ScrollSpeed -= SCROLL_SPEED_CHANGE;

            }

            if (InputManager.IsKeyDown(Keys.PageUp))
            {
                if (lastPlacing == Placing.MOVING_PLATFORMS)
                    Step += STEP_CHANGE;
                else if (lastPlacing == Placing.SCROLLING)
                    ScrollSpeed += SCROLL_SPEED_CHANGE;
            }

            if (InputManager.IsKeyDown(Keys.Home))
            {
                if (lastPlacing == Placing.MOVING_PLATFORMS)
                    AxisFlippedFlag = !AxisFlippedFlag;
                else if (lastPlacing == Placing.SCROLLING)
                    IsLeft = !IsLeft;
            }

            if (InputManager.IsKeyDown(Keys.End))
            {
                GhostPlatformFlag = !GhostPlatformFlag;
            }

            base.PreLayerTick(dt);
        }

        public override void PostLayerTick(double dt)
        {
            base.PostLayerTick(dt);
        }

        public override void PreLayerRender(Camera camera)
        {
            base.PreLayerRender(camera);
        }

        public override void PostLayerRender(Camera camera)
        {
            foreach (Platform platform in Data.Platforms)
            {
                if (!platform.Sprite.IsAwake)
                {
                    TriangulationService.Triangulate(platform.Collider.GetVerticies(), TriangulationMethod.EAR_CLIPPING, out int[] indicies);
                    Renderer.RenderPolygonWireframe(platform.Collider.GetVerticies(), indicies, Color.Red, camera, null, 2);
                }
            }

            foreach (ScrapVector position in Data.TestPositions)
            {
                Renderer.RenderSprite(AssetManager.FetchTexture(TEST_TEXTURE_NAME), position, camera);
            }

            foreach (MovingPlatform platform in Data.MovingPlatforms)
            {
                if (!platform.AxisFlippedFlag)
                {
                    Renderer.RenderLine(platform.Transform.Position - new ScrapVector(platform.Transform.Dimensions.X / 2 + platform.Extent, 0), platform.Transform.Position + new ScrapVector(platform.Transform.Dimensions.X / 2 + platform.Extent, 0), Color.Green, camera, null, 5);
                }
                else
                {
                    Renderer.RenderLine(platform.Transform.Position - new ScrapVector(0, platform.Transform.Dimensions.Y / 2 + platform.Extent), platform.Transform.Position + new ScrapVector(0, platform.Transform.Dimensions.Y / 2 + platform.Extent), Color.Green, camera, null, 5);
                }
                
            }

            if (lastPlacing == Placing.MOVING_PLATFORMS)
            {
                if (!AxisFlippedFlag)
                {
                    Renderer.RenderLine(Transform.Position - new ScrapVector(Transform.Dimensions.X / 2 + Extent, 0), Transform.Position + new ScrapVector(Transform.Dimensions.X / 2 + Extent, 0), Color.Green, camera, null, 5);
                }
                else
                {
                    Renderer.RenderLine(Transform.Position - new ScrapVector(0, Transform.Dimensions.Y / 2 + Extent), Transform.Position + new ScrapVector(0, Transform.Dimensions.Y / 2 + Extent), Color.Green, camera, null, 5);
                }
                
            }

            base.PostLayerRender(camera);
        }
    }
}
