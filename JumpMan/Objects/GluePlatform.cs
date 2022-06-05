using JumpMan.ECS.Components;
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
    public class GluePlatform: Entity 
    {
        public override string Name => "Tileable Platform";

        public Transform2D Transform;
        public Sprite2D Sprite;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;

        public List<Player> Players;

        public GluePlatform(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform2D
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

        public override void PreLayerTick(double dt)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                Player player = Players[i];
                try
                {
                    if (Collision.IntersectPolygons(Collider.GetVerticies(), player.Collider.GetVerticies(), out CollisionManifold manifold))
                    {

                        player.Controller.MoveForce = 50;
                        player.Controller.JumpForce = 2500;
                        player.Controller.JumpDirectionalDegree = 60;

                    }

                    else
                    {
                        player.Controller.MoveForce = Controller.DEFAULT_MOVE_FORCE;
                        player.Controller.JumpForce = Controller.DEFAULT_JUMP_FORCE;
                        player.Controller.JumpDirectionalDegree = Controller.DEFAULT_JUMP_DIRECTIONAL_DEGREE;
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
