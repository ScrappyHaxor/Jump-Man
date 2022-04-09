using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using ScrapBox.Framework.Shapes;

namespace LevelEditor.UI
{
    public class EditorTextbox : Entity
    {
        public override string Name => "Editor Textbox";

        public Transform Transform;
        public Textbox TextBox;

        public EditorTextbox(ScrapVector position, ScrapVector dimensions, string placeholder)
        {
            Transform = new Transform()
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            TextBox = new Textbox
            {
                Placeholder = placeholder,
                Font = AssetManager.FetchFont("editorButton"),
                BorderColor = Color.White,
                FocusColor = Color.Gray,
                PlaceholderColor = Color.Gray,
                Shape = ScrapRect.CreateFromCenter(Transform.Position, Transform.Dimensions),
                OutlineThickness = 2
            };

            RegisterComponent(TextBox);
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
