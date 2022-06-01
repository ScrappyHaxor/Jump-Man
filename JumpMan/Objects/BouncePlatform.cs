using JumpMan.Core;
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
    public class BouncePlatform : Entity
    {
        public override string Name => "Tileable Platform";

        public Transform Transform;
        public Sprite2D Sprite;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;

        public List<Player> Players;

        public BouncePlatform(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
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

            Players = new List<Player>();
        }

        public override void Awake()
        {
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
            for (int i = 0; i < Players.Count; i++)
            {
                Player player = Players[i];
                try
                {
                    prev = curr;
                    curr = player.RigidBody.Grounded();

                    bool collided = Collision.IntersectPolygons(Collider.GetVerticies(), player.Collider.GetVerticies(), out CollisionManifold manifold);

                    if (collided && hasBounced == false && prev == false && curr == true)
                    {
                        player.RigidBody.AddForce(new ScrapVector(player.Controller.jumpForce.X * 0.2f, player.Controller.jumpForce.Y * 0.5f));

                        hasBounced = true;
                    }

                    if (player.RigidBody.Grounded() && hasBounced == true)
                    {
                        hasBounced = false;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
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
