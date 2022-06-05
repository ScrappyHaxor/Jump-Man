using JumpMan.Container;
using JumpMan.ECS.Components;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using static ScrapBox.Framework.ECS.Collider;

namespace JumpMan.Objects
{
    public class Player : Entity
    {
        public const double SIZE_X = 64;
        public const double SIZE_Y = 64;

        //Used in the future for animation. Basically the engine scales the player up or down depending on SIZE_X and SIZE_Y but needs to know the actual texture
        //Cell size for each square in the animation sheet.
        public const int CELL_SIZE_X = 64;
        public const int CELL_SIZE_Y = 64;

        public override string Name => "Player";

        public Transform2D Transform;
        public RigidBody2D RigidBody;
        public BoxCollider2D Collider; // Maybe use a CircleCollider2D instead in the future.
        public Sprite2D Sprite;
        public KeyboardController Controller; // Make this a generic controller once other controllers are implemented.

        private SettingsData settingsData;

        public Player(ScrapVector position) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform2D
            {
                Position = position,
                Dimensions = new ScrapVector(SIZE_X, SIZE_Y)
            };

            RegisterComponent(Transform);

            RigidBody = new RigidBody2D
            {
                Mass = 6,
                Restitution = 0.0f,
                Drag = 0.2,
                Friction = 4.0
            }; // Most of these properties are either broken or not working correctly
            //Not all things in the framework work perfectly

            RegisterComponent(RigidBody);

            Collider = new BoxCollider2D
            {
                Dimensions = Transform.Dimensions,
                Algorithm = CollisionAlgorithm.SAT
            };

            RegisterComponent(Collider);

            Sprite = new Sprite2D
            {
                TintColor = Color.White,
                SourceRectangle = new Rectangle(0, 0, CELL_SIZE_X, CELL_SIZE_Y)
            };

            RegisterComponent(Sprite);

            Controller = new KeyboardController();

            RegisterComponent(Controller);
        }

        public override void Awake()
        {
            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
            {
                settingsData = new SettingsData();
                settingsData.SaveSettings();
            }

            Sprite.Texture = AssetManager.FetchTexture(settingsData.CosmeticInUse);
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
            base.PostLayerRender(camera);
        }
    }
}
