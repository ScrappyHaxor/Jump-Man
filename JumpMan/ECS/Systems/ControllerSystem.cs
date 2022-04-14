using JumpMan.ECS.Components;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.ECS.Systems
{
    public class ControllerSystem : ComponentSystem
    {
        private List<Controller> Controllers;

        public ControllerSystem()
        {
            Controllers = new List<Controller>();
        }

        public void RegisterController(Controller controller)
        {
            Controllers.Add(controller);
        }

        internal void PurgeController(Controller controller)
        {
            Controllers.Remove(controller);
        }

        public override void Reset()
        {
            Controllers.Clear();
        }

        public override void Tick(double dt)
        {
            for (int i = 0; i < Controllers.Count; i++)
            {
                Controllers[i].TakeInput();
            }
        }

        public override void Render(Camera mainCamera)
        {
            //Controllers shouldnt be rendered.
            return;
        }
    }
}
