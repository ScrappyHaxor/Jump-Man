using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using Microsoft.Xna.Framework;
using Rectangle = ScrapBox.Framework.Shapes.Rectangle;

namespace ModTools.UI
{
    public class EditorTextbox : Entity
    {
        public override string Name => "Editor Textbox";

        public Transform2D Transform;
        public Textbox2D TextBox;

        public EditorTextbox(ScrapVector position, ScrapVector dimensions, string placeholder) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform2D()
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            TextBox = new Textbox2D
            {
                Placeholder = placeholder,
                Font = AssetManager.FetchFont("editorButtonText"),
                BorderColor = Color.White,
                FocusColor = Color.Gray,
                PlaceholderColor = Color.Gray,
                Shape = new Rectangle(Transform.Position, Transform.Dimensions),
                OutlineThickness = 2
            };

            RegisterComponent(TextBox);
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
