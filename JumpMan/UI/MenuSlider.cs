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
    public class MenuSlider : Entity
    {
        public override string Name => "Menu Slider";

        public Transform Transform;
        public Slider Slider;

        public MenuSlider(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform()
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Slider = new Slider()
            {
                BarColor = Color.Gray,
                BarBorderColor = Color.White,
                HandleColor = Color.Red,
                HandleHoverColor = Color.Blue,
                Font = AssetManager.FetchFont("temporary"),
                HandlePoints = 20,
                HandleRadius = 12
            };

            RegisterComponent(Slider);
        }

        public override void Awake()
        {
            base.Awake();
        }
    }
}
