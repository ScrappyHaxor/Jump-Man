using JumpMan.Container;
using JumpMan.Objects;
using ModTools.Core;
using ModTools.ECS.Components;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
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

        public List<string> PlatformTextures = new List<string>()
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
            "placeholder4"
        };

        public int PlatformTextureIndex;
        public int BackgroundTextureIndex;
        public int TrapTextureIndex;

        public override string Name => "Editor Ghost";

        public Transform Transform;
        public Sprite2D Sprite;

        public LevelData Data;


        public EditorGhost() : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
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
                platform.Awake();
                platform.Sprite.Mode = Sprite.Mode;

                Data.Platforms.Add(platform);
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
                IllusionPlatform trap = new IllusionPlatform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.KNOCKBACK_LEFT)
            {
                KnockBackPlatformLeft trap = new KnockBackPlatformLeft(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.KNOCKBACK_RIGHT)
            {
                KnockBackPlatformRight trap = new KnockBackPlatformRight(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
            }
            else if (placingState == Placing.BOUNCE)
            {
                FeetBouncePlatform trap = new FeetBouncePlatform(Sprite.Texture.Name, Transform.Position, Transform.Dimensions);
                trap.Awake();

                Data.Traps.Add(trap);
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
            else if (placingState == Placing.BACKGROUNDS)
            {
                BackgroundTextureIndex++;
                if (BackgroundTextureIndex > BackgroundTextures.Count - 1)
                {
                    BackgroundTextureIndex = 0;
                }

                Sprite.Texture = AssetManager.FetchTexture(BackgroundTextures[BackgroundTextureIndex]);
            }
            else if (placingState == Placing.GLUE || placingState == Placing.ILLUSION || placingState == Placing.KNOCKBACK_LEFT ||
                placingState == Placing.KNOCKBACK_RIGHT || placingState == Placing.BOUNCE)
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
            else if (placingState == Placing.BACKGROUNDS)
            {
                BackgroundTextureIndex--;
                if (BackgroundTextureIndex < 0)
                {
                    BackgroundTextureIndex = BackgroundTextures.Count - 1;
                }

                Sprite.Texture = AssetManager.FetchTexture(BackgroundTextures[BackgroundTextureIndex]);
            }
            else if (placingState == Placing.GLUE || placingState == Placing.ILLUSION || placingState == Placing.KNOCKBACK_LEFT ||
                placingState == Placing.KNOCKBACK_RIGHT || placingState == Placing.BOUNCE)
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
            if (placingState == Placing.PLATFORMS)
            {
                Sprite.Texture = AssetManager.FetchTexture(PlatformTextures[PlatformTextureIndex]);
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
            else if (placingState == Placing.GLUE)
            {
                Sprite.Texture = AssetManager.FetchTexture(TrapTextures[TrapTextureIndex]);
                Transform.Dimensions = new ScrapVector(Sprite.Texture.Width, Sprite.Texture.Height);
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
            foreach (ScrapVector position in Data.TestPositions)
            {
                Renderer.RenderSprite(AssetManager.FetchTexture(TEST_TEXTURE_NAME), position, camera);
            }

            base.PostLayerRender(camera);
        }
    }
}
