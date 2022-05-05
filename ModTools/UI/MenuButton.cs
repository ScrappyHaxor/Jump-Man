using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Shapes;
using System;
using System.Collections.Generic;
using System.Text;
using Rectangle = ScrapBox.Framework.Shapes.Rectangle;

namespace ModTools.UI
{
    public class MenuButton : Entity
    {
        public override string Name => "UI Button";

        public Transform Transform;
        public Label Label;
        public Button Button;

        public MenuButton(ScrapVector position, ScrapVector dimensions, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Button = new Button
            {
                BorderColor = Color.White,
                HoverColor = Color.Gray,
                FillColor = Color.White,
                Shape = new Rectangle(Transform.Position, Transform.Dimensions),
                OutlineThickness = 2
            };

            RegisterComponent(Button);

            Label = new Label
            {
                Font = AssetManager.FetchFont("menuButton"),
                Text = text,
                TextColor = Color.Black
            };

            RegisterComponent(Label);
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
