using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.Objects
{
    public class Background : Entity
    {
        public override string Name => "Tileable Background";

        public Transform2D Transform;
        public Sprite2D Sprite;
        public RigidBody2D RigidBody;
        public BoxCollider2D Collider;

        public Background(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.BACKGROUND))
        {
            Transform = new Transform2D
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Sprite = new Sprite2D
            {
                Texture = AssetManager.FetchTexture(texture),
                Mode = SpriteMode.TILE
            };

            RegisterComponent(Sprite);

            RigidBody = new RigidBody2D
            {
                IsStatic = true
            };

            RegisterComponent(RigidBody);

            Collider = new BoxCollider2D
            {
                Dimensions = Transform.Dimensions
            };

            RegisterComponent(Collider);
        }

        public override void Awake()
        {
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
