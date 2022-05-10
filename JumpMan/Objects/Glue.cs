using JumpMan.ECS.Components;
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
    public class Glue: Entity 
    {
        public override string Name => "Tileable Platform";

        public Transform Transform;
        public Sprite2D Sprite;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;
        public Player Player;
        
        
        bool success;

       


        public Glue(string texture, ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
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

        public  void GlueP()
        {
            if (Collision.IntersectPolygons(Collider.GetVerticies(), Player.Collider.GetVerticies(), out CollisionManifold manifold))
            {

                Player.Controller.MoveForce = 50;
                Player.Controller.JumpForce = 1500;
                Player.Controller.JumpDirectionalDegree = 60;

            }

            else {
                Player.Controller.MoveForce = Controller.DEFAULT_MOVE_FORCE;
                Player.Controller.JumpForce = Controller.DEFAULT_JUMP_FORCE;
                Player.Controller.JumpDirectionalDegree = Controller.DEFAULT_JUMP_DIRECTIONAL_DEGREE;
                }
        }

        public override void Awake()
        {

            success = Dependency<Player>(out Player);
            if (!success)
                return;

            base.Awake();
        }

        public override void Sleep()
        {
            base.Sleep();
        }

        public override void PreLayerTick(double dt)
        {
            GlueP();
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
