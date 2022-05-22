using System;
using System.Collections.Generic;
using System.Text;

using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.UI
{
    class GenericProgressBar : Entity
    {
        public override string Name => "Jump Power";

        public Transform Transform;
        public ProgressBar ProgressBar;

        public GenericProgressBar(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };
            RegisterComponent(Transform);

            ProgressBar = new ProgressBar
            {

            };
            RegisterComponent(ProgressBar);
        }

        public override void Awake()
        {
            base.Awake();
        }
    }
}
