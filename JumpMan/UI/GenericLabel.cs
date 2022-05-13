using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.UI
{
    public class GenericLabel : Entity
    {
        public override string Name => "Menu Label";

        public Transform Transform;
        public Label Label;

        public GenericLabel(ScrapVector position, ScrapVector dimensions, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Label = new Label
            {
                Font = AssetManager.FetchFont("temporary"),
                Text = text,
                TextColor = new Color(223, 246, 255)
            };

            RegisterComponent(Label);
        }

        
    }
}
