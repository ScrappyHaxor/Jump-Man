using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.UI
{
    public class EditorLabel : Entity
    {
        public override string Name => "Editor Label";

        public Transform Transform;
        public Label Label;

        public EditorLabel(ScrapVector position, string text)
        {
            Transform = new Transform
            {
                Position = position
            };

            RegisterComponent(Transform);

            Label = new Label
            {
                Text = text,
                TextColor = Color.White,
                Font = AssetManager.FetchFont("editorButton")
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
