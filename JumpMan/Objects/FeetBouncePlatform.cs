using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;
using static ScrapBox.Framework.ECS.Collider;

namespace JumpMan.Objects
{
    public class FeetBouncePlatform : Entity
    {
        public override string Name => "Tileable Platform";

        Player Player;

        public Transform Transform;
        public Sprite2D Sprite;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;

        public FeetBouncePlatform(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Rigidbody = new RigidBody2D
            {
                IsStatic = true
            };

            RegisterComponent(Rigidbody);

            Collider = new BoxCollider2D
            {
                Algorithm = CollisionAlgorithm.SAT,
                Dimensions = dimensions
            };

            RegisterComponent(Collider);

            Sprite = new Sprite2D
            {
                Texture = AssetManager.FetchTexture(texture),
                Mode = SpriteMode.TILE
            };

            RegisterComponent(Sprite);
        }

        public override void Awake()
        {
            bool success = Dependency<Player>(out Player);

            base.Awake();
        }

        public override void Sleep()
        {
            base.Sleep();
        }

        //temporary test variables
        bool hasBounced = false;

        bool curr = false;
        bool prev;

        public override void PreLayerTick(double dt)
        {
            bool success = Dependency<Player>(out Player);

            prev = curr;
            curr = Player.RigidBody.Grounded();

            if (Player != null)
            {
                bool collided = Collision.IntersectPolygons(Collider.GetVerticies(), Player.Collider.GetVerticies(), out CollisionManifold manifold);

                if (collided && hasBounced == false && prev == false && curr == true)
                {
                    Player.RigidBody.AddForce(new ScrapVector(Player.Controller.jumpForce.X * 0.2f, Player.Controller.jumpForce.Y * 0.5f));

                    hasBounced = true;
                }
            }

            if (Player.RigidBody.Grounded() && hasBounced == true)
            {
                hasBounced = false;
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
            base.PostLayerRender(camera);
        }
    }
}
