using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using static ScrapBox.Framework.ECS.Collider;

namespace JumpMan.Objects
{
    public class MovingPlatform : Entity
    {
        public override string Name => "Tileable Platform";

        public Transform2D Transform;
        public Sprite2D Sprite;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;

        public List<Player> Players;

        public double Extent = 100;
        public double Step = 1;

        public bool OverrideFlag;
        public bool AxisFlippedFlag;

        public bool Server;

        public ScrapVector startPosition;
        private ScrapVector vector;

        public MovingPlatform(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform2D
            {
                Position = position,
                Dimensions = dimensions
            };

            startPosition = position;
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
            if (!AxisFlippedFlag)
            {
                vector = new ScrapVector(Step, 0);
            }
            else
            {
                vector = new ScrapVector(0, Step);
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

            if (OverrideFlag)
                return;

            Transform.Position += vector;

            if (!AxisFlippedFlag)
            {
                if (Transform.Position.X < startPosition.X - Extent)
                {
                    vector = new ScrapVector(Step, 0);
                }
                else if (Transform.Position.X > startPosition.X + Extent)
                {
                    vector = new ScrapVector(-Step, 0);
                }
            }
            else
            {
                if (Transform.Position.Y < startPosition.Y - Extent)
                {
                    vector = new ScrapVector(0, Step);
                }
                else if (Transform.Position.Y > startPosition.Y + Extent)
                {
                    vector = new ScrapVector(0, -Step);
                }
            }

            for (int i = 0; i < Players.Count; i++)
            {
                Player player = Players[i];
                try
                {
                    if (Collision.IntersectPolygons(Collider.GetVerticies(), player.Collider.GetVerticies(), out CollisionManifold manifold))
                    {
                        if (vector == new ScrapVector(0, Step))
                            return;

                        player.Transform.Position += vector;
                    }
                }
                catch (NullReferenceException)
                {
                    continue;
                }
            }
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