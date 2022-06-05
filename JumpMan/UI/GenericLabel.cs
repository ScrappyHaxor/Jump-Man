using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace JumpMan.UI
{
    public class GenericLabel : Entity
    {
        public override string Name => "Menu Label";

        public Transform2D Transform;
        public Label2D Label;

        public GenericLabel(ScrapVector position, ScrapVector dimensions, string text) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Transform = new Transform2D
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Label = new Label2D
            {
                Font = AssetManager.FetchFont("temporary"),
                Text = text,
                TextColor = new Color(223, 246, 255)
            };

            RegisterComponent(Label);
        }

        
    }
}
