using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;
using static ScrapBox.Framework.ECS.Collider;

namespace JumpMan.Objects
{
    public class EndOfLevel : Entity
    {
        public override string Name => "End of Level";

        public Transform Transform;
        public RigidBody2D Rigidbody;
        public BoxCollider2D Collider;
        public Sprite2D Sprite;

        public bool OverrideFlag;
        public bool TestFlag;
        public bool EditorFlag;
        public object[] Container;

        private bool triggered;

        public EndOfLevel(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Rigidbody = new RigidBody2D
            {
                IsStatic = true
            };

            RegisterComponent(Rigidbody);

            Collider = new BoxCollider2D
            {
                Algorithm = CollisionAlgorithm.SAT,
                Dimensions = Transform.Dimensions,
                Trigger = TriggerType.TRIGGER_ONLY,
                Triggered = EnteredEndOfLevel
            };

            RegisterComponent(Collider);

            Sprite = new Sprite2D
            {
                Texture = AssetManager.FetchTexture("placeholder5"),
                Mode = SpriteMode.TILE
            };

            RegisterComponent(Sprite);
        }

        public override void Awake()
        {
            triggered = false;
            base.Awake();
        }

        public void EnteredEndOfLevel(object o, EventArgs e)
        {
            if (triggered)
                return;

            triggered = true;
            if (OverrideFlag)
            {
                if (TestFlag)
                {
                    SceneManager.SwapScene("tool menu");
                }
                
                if (EditorFlag)
                {
                    SceneManager.SwapScene("editor", Container);
                }
            }
            else
            {
                SceneManager.SwapScene("Main Menu");
            }
        }
    }
}
