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
    public class GenericSlider : Entity
    {
        public override string Name => "Menu Slider";

        public Transform Transform;
        public Slider Slider;

        public GenericSlider(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform()
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Slider = new Slider()
            {
                BarColor = new Color(5, 19, 103),
                BarBorderColor = new Color(93, 139, 244),
                HandleColor = Color.White,
                HandleHoverColor = Color.Gray,
                Font = AssetManager.FetchFont("temporary"),
                HandlePoints = 20,
                HandleRadius = dimensions.Y,
                LineThickness = 3
            };

            RegisterComponent(Slider);
        }

        public override void Awake()
        {
            base.Awake();
        }
    }
}
