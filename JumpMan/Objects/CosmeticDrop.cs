using JumpMan.Container;
using JumpMan.UI;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;

namespace JumpMan.Objects
{
    public class CosmeticDrop : Entity
    {
        public override string Name => "Cosmetic Drop";

        public const int CELL_SIZE_X = 64;
        public const int CELL_SIZE_Y = 64;

        public const int SIZE_X = 64;
        public const int SIZE_Y = 64;

        public Transform2D Transform;
        public RigidBody2D Rigidbody;
        public Collider Collider;
        public Sprite2D Sprite;

        private bool unlocked;

        public CosmeticDrop(ScrapVector position, string cosmetic) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.FOREGROUND))
        {
            Transform = new Transform2D
            {
                Position = position,
                Dimensions = new ScrapVector(SIZE_X, SIZE_Y)
            };

            RegisterComponent(Transform);

            Rigidbody = new RigidBody2D
            {
                IsStatic = true
            };

            RegisterComponent(Rigidbody);

            Collider = new BoxCollider2D
            {
                Dimensions = Transform.Dimensions,
                Algorithm = Collider.CollisionAlgorithm.SAT,
                Trigger = Collider.TriggerType.TRIGGER_ONLY,
                Triggered = UnlockCosmetic
            };

            RegisterComponent(Collider);

            Sprite = new Sprite2D
            {
                Texture = AssetManager.FetchTexture(cosmetic),
                Mode = SpriteMode.SCALE,
                SourceRectangle = new Rectangle(0, 0, CELL_SIZE_X, CELL_SIZE_Y)
            };

            RegisterComponent(Sprite);
        }

        public void UnlockCosmetic(object o, EventArgs e)
        {
            if (unlocked)
                return;

            unlocked = true;
            SettingsData settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
            {
                settingsData = new SettingsData();
                settingsData.SaveSettings();
            }

            int index = -1;
            for (int i = 0; i < CosmeticsOverlay.TextureNameTable.Length; i++)
            {
                if (CosmeticsOverlay.TextureNameTable[i] == Sprite.Texture.Name)
                {
                    index = i;
                    break;
                }
            }

            settingsData.CosmeticStatus[index] = true;
            settingsData.SaveSettings();

            Sleep();
        }
    }
}
