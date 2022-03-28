using JumpMan.ECS.Components;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;
using static ScrapBox.Framework.ECS.Collider;

namespace JumpMan.Objects
{
    public class Platform : Entity
    {
        public override string Name => "Platform";

        public Transform Transform;
        public RigidBody2D RigidBody;
        public BoxCollider2D Collider; // Maybe use a CircleCollider2D instead in the future.
        public Sprite2D Sprite;

        public Platform(ScrapVector position, ScrapVector dimensions)
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            RigidBody = new RigidBody2D
            {
                Mass = 1,
                Restitution = 0f,
                Drag = 0.9,
                Friction = 0.9,
                IsStatic = true
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
                Texture = AssetManager.FetchTexture("placeholder"),
                TintColor = Color.Blue
            };

            RegisterComponent(Sprite);
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Sleep()
        {
            base.Sleep();
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Draw(Camera mainCamera)
        {
            //Remember, drawing the platforms is automatically handled by the framework.
            base.Draw(mainCamera);
        }
    }
}
