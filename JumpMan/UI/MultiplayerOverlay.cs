using JumpMan.Container;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace JumpMan.UI
{
    public class MultiplayerOverlay : EntityCollection
    {
        public const int OffsetY = -500;

        public const int ButtonXSize = 850;
        public const int ButtonYSize = 100;
        public const int ButtonYOffset = ButtonYSize + 40;

        public override List<Entity> Register { get; set; }

        public GenericLabel TitleLabel;
        public GenericButton Host;
        public GenericButton Join;
        public GenericTextbox IpTextbox;
        public GenericButton BackButton;

        public MultiplayerOverlay() : base(SceneManager.CurrentScene.Stack.Fetch(3))
        {
            TitleLabel = new GenericLabel(new ScrapVector(0, OffsetY - 60), new ScrapVector(ButtonXSize, ButtonYSize), "Multiplayer");
            TitleLabel.Label.Font = AssetManager.FetchFont("temporaryBiggest");
            TitleLabel.Layer = layer;
            Register.Add(TitleLabel);

            Host = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), new ScrapVector(ButtonXSize, ButtonYSize), "Host");
            Host.Layer = layer;
            Host.Label.Font = AssetManager.FetchFont("temporaryBigger");
            Host.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("Multiplayer Level", "level1.data", true, IpTextbox.Textbox.GetText);
            };
            Register.Add(Host);

            IpTextbox = new GenericTextbox(new ScrapVector(0, OffsetY + ButtonYOffset * 2), new ScrapVector(ButtonXSize, ButtonYSize));
            IpTextbox.Layer = layer;
            IpTextbox.Textbox.Font = AssetManager.FetchFont("temporaryBigger");
            IpTextbox.Textbox.Centered = true;
            IpTextbox.Textbox.Placeholder = "127.0.0.1";
            IpTextbox.Textbox.OutlineThickness = 3;
            Register.Add(IpTextbox);

            Join = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 3), new ScrapVector(ButtonXSize, ButtonYSize), "Join");
            Join.Layer = layer;
            Join.Label.Font = AssetManager.FetchFont("temporaryBigger");
            Join.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("Multiplayer Level", "level1.data", false, IpTextbox.Textbox.GetText);
            };
            Register.Add(Join);

            BackButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 4), new ScrapVector(ButtonXSize, ButtonYSize), "Back to Menu");
            BackButton.Layer = layer;
            BackButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
            BackButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("Main Menu");
            };
            Register.Add(BackButton);
        }
    }
}
