using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using JumpMan.ECS.Systems;

namespace JumpMan.ECS.Components
{
    //We abstract controller because a keyboard controller class wont have the same keybindings as an xbox controller class
    public abstract class Controller : Component
    {
        public const double DEFAULT_MOVE_FORCE = 100;
        public const double DEFAULT_JUMP_FORCE = 3000;
        public const double DEFAULT_MIN_JUMP_MULTIPLIER = 1;
        public const double DEFAULT_MAX_JUMP_MULTIPLIER = 3;
        public const double DEFAULT_JUMP_DIRECTIONAL_DEGREE = 30;

        public double MoveForce = DEFAULT_MOVE_FORCE;
        public double JumpForce = DEFAULT_JUMP_FORCE;
        public double MinJumpMultiplier = DEFAULT_MIN_JUMP_MULTIPLIER;
        public double MaxJumpMultiplier = DEFAULT_MAX_JUMP_MULTIPLIER;
        public double JumpDirectionalDegree = DEFAULT_JUMP_DIRECTIONAL_DEGREE;

        public string SelectedLevel;

        protected RigidBody2D rigidbody;
        protected Sprite2D sprite;

        public override void Awake()
        {
            if (IsAwake)
                return;

            //My library makes it easy for components to require other components to function
            //The player controller needs a Rigidbody2D so it can just require it.
            //If the rigidbody isnt attached to the player or isnt awake it will cause a console error and put itself back to sleep.
            bool success = Dependency(out rigidbody, true);
            success = Dependency(out sprite, true);

            if (Layer == null)
                Layer = Owner.Layer;

            //The component registers itself to the ControllerSystem on the appropriate layer
            Layer.GetSystem<ControllerSystem>().RegisterController(this);

            IsAwake = true;
        }

        public override void Sleep()
        {
            if (!IsAwake)
                return;

            Layer.GetSystem<ControllerSystem>().PurgeController(this);

            IsAwake = false;
        }

        public abstract void TakeInput();
    }
}
