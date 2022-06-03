using System;
using System.Collections.Generic;
using System.Text;

using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.UI
{
    class GameplayOverlay : EntityCollection
    {
        public override List<Entity> Register { get; set; }

        private GenericProgressBar genericProgressBar;

        public GameplayOverlay() : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            genericProgressBar = new GenericProgressBar(
                new ScrapVector(0, 0),
                new ScrapVector(1000, 50),
                300
                );
            Register.Add(genericProgressBar);
        }
    }
}
