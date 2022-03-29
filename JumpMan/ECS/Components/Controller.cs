using System;
using System.Collections.Generic;
using ScrapBox.Framework.ECS;
using System.Text;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using JumpMan.ECS.Systems;

namespace JumpMan.ECS.Components
{
    //We abstract controller because a keyboard controller class wont have the same keybindings as an xbox controller class
    public abstract class Controller : Component
    {
        public const double MOVE_FORCE = 100;
        public const double JUMP_FORCE = 3000;
        public const double MIN_JUMP_MULTIPLIER = 1;
        public const double MAX_JUMP_MULTIPLIER = 3;
        public const double JUMP_DIRECTIONAL_DEGREE = 45;

        protected RigidBody2D rigidbody;

        public override void Awake()
        {
            if (IsAwake)
                return;

            //My library makes it easy for components to require other components to function
            //The player controller needs a Rigidbody2D so it can just require it.
            //If the rigidbody isnt attached to the player or isnt awake it will cause a console error and put itself back to sleep.
            bool success = Dependency(out rigidbody);
            if (!success)
                return;

            //The component registers itself to the ControllerSystem
            WorldManager.GetSystem<ControllerSystem>().RegisterController(this);

            IsAwake = true;
        }

        public override void Sleep()
        {
            if (!IsAwake)
                return;

            WorldManager.GetSystem<ControllerSystem>().PurgeController(this);

            IsAwake = false;
        }

        public abstract void TakeInput();
    }
}
