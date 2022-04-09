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

namespace LevelEditor.UI
{
    public class EditorButton : Entity
    {
        public override string Name => "Editor Button";

        public Transform Transform;
        public Label Label;
        public Button Button;

        public EditorButton(ScrapVector position, ScrapVector dimensions, string text)
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Label = new Label
            {
                Font = AssetManager.FetchFont("editorButton"),
                Text = text,
                TextColor = Color.White
            };

            RegisterComponent(Label);

            Button = new Button
            {
                BorderColor = Color.White,
                HoverColor = Color.Gray,
                Shape = ScrapRect.CreateFromCenter(Transform.Position, Transform.Dimensions),
                OutlineThickness = 2
            };

            RegisterComponent(Button);
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
            base.Draw(mainCamera);
        }
    }
}
