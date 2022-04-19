using LevelEditor.ECS.Components;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Objects
{
    public class EditorPlayer : Entity
    {
        public override string Name => "Editor Player";

        public EditorController Controller;

        public EditorPlayer() : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Controller = new EditorController()
            {
                Camera = SceneManager.CurrentScene.MainCamera
            };

            RegisterComponent(Controller);
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
