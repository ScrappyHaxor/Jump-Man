using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using Rectangle = ScrapBox.Framework.Shapes.Rectangle;

namespace JumpMan.UI
{
    public class GenericButton : Entity
    {
        public override string Name => "Main Menu Button";

        public Transform Transform;
        public Label Label;
        public Button Button;

        public GenericButton(ScrapVector position, ScrapVector dimensions, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Button = new Button
            {
                BorderColor = new Color(93, 139, 244),
                HoverColor = new Color(45, 49, 250),
                FillColor = new Color(5, 19, 103),
                Shape = new Rectangle(Transform.Position, Transform.Dimensions),
                OutlineThickness = 2
            };

            RegisterComponent(Button);

            Label = new Label
            {
                Font = AssetManager.FetchFont("temporary"),
                Text = text,
                TextColor = new Color(223, 246, 255)
            };

            RegisterComponent(Label);
        }

        //When awake is called, the entity is registered to the WorldManager and is added to the game loop
        public override void Awake()
        {
            base.Awake();
        }

        
        //When sleep is called, the entity is de-registered from the world manager and is removed from the game loop
        public override void Sleep()
        {
            base.Sleep();
        }

        public override void PreLayerTick(double dt)
        {
            Button.Transform.Position = Transform.Position;
            Button.Transform.Dimensions = Transform.Dimensions;
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
