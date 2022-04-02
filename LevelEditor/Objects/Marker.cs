using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Objects
{
    public class Marker : Entity
    {
        public override string Name => "Marker";

        public Transform Transform;
        public Sprite2D Sprite;

        public Marker(ScrapVector position, ScrapVector dimensions)
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Sprite = new Sprite2D
            {
                Texture = AssetManager.FetchTexture("placeholder"),
                TintColor = Color.Red
            };

            RegisterComponent(Sprite);
        }
    }
}
