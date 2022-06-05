using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace ModTools.UI
{
    public class EditorLabel : Entity
    {
        public override string Name => "Editor Label";

        public Transform2D Transform;
        public Label2D Label;

        public EditorLabel(ScrapVector position, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform2D
            {
                Position = position
            };

            RegisterComponent(Transform);

            Label = new Label2D
            {
                Text = text,
                TextColor = Color.White,
                Font = AssetManager.FetchFont("editorButtonText")
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
