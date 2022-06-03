using System;
using System.Collections.Generic;
using System.Text;

using JumpMan.Objects;

using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.UI
{
    class GenericProgressBar : Entity
    {
        public override string Name => "Jump Power";

        Player player;

        double zdt = 0;

        public Transform Transform;
        public ProgressBar ProgressBar;

        public GenericProgressBar(ScrapVector position, ScrapVector dimensions, int maxValue, int minValue = 0) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };
            RegisterComponent(Transform);

            ProgressBar = new ProgressBar()
            {
                MaxValue = maxValue
            };
            RegisterComponent(ProgressBar);
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void PreLayerTick(double dt)
        {
            //  wasteful but this component is loaded before the player
            bool success = Dependency<Player>(out player, true, true);

            if (player.RigidBody.Grounded())
            {
                if (InputManager.IsKeyHeld(Keys.Space))
                {
                    zdt+=dt;

                    ProgressBar.SetValue(zdt);
                }
            }
            else
            {
                ProgressBar.Reset();
                zdt = 0;
            }

            base.PostLayerTick(dt);
        }
    }
}
