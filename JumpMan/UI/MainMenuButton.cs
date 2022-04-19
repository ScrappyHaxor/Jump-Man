﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using ScrapBox.Framework.Shapes;

namespace JumpMan.UI
{
    public class MainMenuButton : Entity
    {
        public override string Name => "Main Menu Button";

        public Transform Transform;
        public Label Label;
        public Button Button;

        public MainMenuButton(ScrapVector position, ScrapVector dimensions, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
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
                Text = text
            };

            RegisterComponent(Label);

            Button = new Button
            {
                BorderColor = Color.White,
                HoverColor = Color.Gray,
                Shape = ScrapRect.CreateFromCenter(Transform.Position, Transform.Dimensions),
            };

            RegisterComponent(Button);
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
