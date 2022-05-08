using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.Objects
{
    public class SomeObject : Entity
    {
        public override string Name => "bruh";

        public Transform Transform;
        public Label label;

        public Player player;

        public SomeObject() : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform()
            {
                Position = ScrapVector.Zero,
                Dimensions = new ScrapVector(10, 10)
            };

            RegisterComponent(Transform);

            label = new Label()
            {
                Text = String.Empty,
                Font = AssetManager.FetchFont("temporary"),
                TextColor = Color.White
            };

            RegisterComponent(label);
        }

        public override void Awake()
        {
            bool success = Dependency(out player);
            if (!success)
                return;


            base.Awake();
        }

        public override void PostLayerTick(double dt)
        {
            Transform.Position = player.Transform.Position;
            label.Text = player.Controller.MoveForce.ToString();
            base.PostLayerTick(dt);
        }
    }
}
