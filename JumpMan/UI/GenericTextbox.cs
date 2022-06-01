using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.UI
{
    public class GenericTextbox : Entity
    {
        public override string Name => "Generic Text Box";

        public Transform Transform;
        public Textbox Textbox;

        public GenericTextbox(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Textbox = new Textbox
            {
                Font = AssetManager.FetchFont("temporary"),
                BorderColor = new Color(93, 139, 244),
                FocusColor = new Color(45, 49, 250)
            };

            RegisterComponent(Textbox);
        }
    }
}
