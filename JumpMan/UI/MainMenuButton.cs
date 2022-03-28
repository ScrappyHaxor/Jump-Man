using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Shapes;

namespace JumpMan.UI
{
    public class MainMenuButton : Entity
    {
        public override string Name => "Main Menu Button";

        private Transform Transform;
        private Label Label;
        public Button Button;

        public MainMenuButton(ScrapVector position, ScrapVector dimensions, string text)
        {
            Transform = new Transform
            {
                Position = position,
                Dimensions = dimensions
            };

            RegisterComponent(Transform);

            Label = new Label
            {
                Font = AssetManager.FetchFont("temporary"),
                Text = text
            };

            RegisterComponent(Label);

            Button = new Button
            {
                BorderColor = Color.White,
                HoverColor = Color.Gray,
                Shape = ScrapRect.CreateFromCenter(Transform.Position, Transform.Dimensions),
            };

            RegisterComponent(Button);
        }

        //When awake is called, the entity is registered to the WorldManager and is added to the game loop
        public override void Awake()
        {
            base.Awake();
        }

        
        //When sleep is called, the entity is de-registered from the world manager and is removed from the game loop
        public override void Sleep()
        {
            base.Sleep();
        }

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Draw(Camera mainCamera)
        {
            //Like ive said before, most stuff is automatically drawn for you so this is only here for advanced rendering
            base.Draw(mainCamera);
        }
    }
}
