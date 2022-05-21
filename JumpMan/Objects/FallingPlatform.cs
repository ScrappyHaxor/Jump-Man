using JumpMan.ECS.Components;
using Microsoft.Xna.Framework;
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
    class FallingPlatform : Entity
    {
        public override string Name => "Tileable Platform";

        private Player player;
        private ScrapVector origin;
        private ScrapVector minPos;
        private double timer = 0d;
        private double zTimer = 2d;
        private bool collisionOccured = false;
        private bool returning = false;

        public Transform Transform;
        public Sprite2D Sprite;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;

        public FallingPlatform(string texture, ScrapVector position, ScrapVector dimensions, ScrapVector minPos) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            #region Components
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
            #endregion

            this.minPos = minPos;
            origin = Transform.Position;
        }

        public override void Awake()
        {
            bool success = Dependency<Player>(out player);

            base.Awake();
        }

        public override void Sleep()
        {
            base.Sleep();
        }

        public override void PreLayerTick(double dt)
        {
            bool collided = Collision.IntersectPolygons(Collider.GetVerticies(), player.Collider.GetVerticies(), out CollisionManifold manifold);

            if(collided && player.RigidBody.Grounded())
            {
                collisionOccured = true;
            }

            if (!returning && collisionOccured && Rigidbody.Transform.Position != minPos)
            {
                Rigidbody.Transform.Position = new ScrapVector(Rigidbody.Transform.Position.X, Rigidbody.Transform.Position.Y + 1);
            }

            if (Rigidbody.Transform.Position == minPos)
            {
                timer += dt;
                collisionOccured = false;

                if (timer >= zTimer)
                {
                    returning = true;
                    timer = 0;
                }
            }

            if (returning)
            {
                Rigidbody.Transform.Position = new ScrapVector(Rigidbody.Transform.Position.X, Rigidbody.Transform.Position.Y - 1);

                if (Rigidbody.Transform.Position.Y <= origin.Y)
                {
                    Rigidbody.Transform.Position = origin;
                    returning = false;
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
