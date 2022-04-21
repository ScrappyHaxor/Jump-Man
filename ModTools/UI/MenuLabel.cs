﻿using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModTools.UI
{
    public class MenuLabel : Entity
    {
        public override string Name => "Menu Label";

        public Transform Transform;
        public Label Label;

        public MenuLabel(ScrapVector position, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform()
            {
                Position = position
            };

            RegisterComponent(Transform);

            Label = new Label()
            {
                TextColor = Color.White,
                Font = AssetManager.FetchFont("menu"),
                Text = text,
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