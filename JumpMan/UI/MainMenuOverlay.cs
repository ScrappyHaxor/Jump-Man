using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.UI
{
    public class MainMenuOverlay : EntityCollection
    {
        public const int OffsetY = -500;

        public const int ButtonXSize = 850;
        public const int ButtonYSize = 100;
        public const int ButtonYOffset = ButtonYSize + 40;

        public override List<Entity> Register { get; set; }

        private GenericLabel title;

        private GenericButton singleplayer;
        private GenericButton multiplayer;
        private GenericButton cosmetics;
        private GenericButton settings;
        private GenericButton quit;

        private readonly SingleplayerOverlay singleplayerOverlay;
        private readonly SettingsOverlay settingsOverlay;

        public MainMenuOverlay(SingleplayerOverlay singleplayerOverlay, SettingsOverlay settingsOverlay) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            this.singleplayerOverlay = singleplayerOverlay;
            this.settingsOverlay = settingsOverlay;

            ScrapVector buttonDimensions = new ScrapVector(ButtonXSize, ButtonYSize);

            title = new GenericLabel(new ScrapVector(0, OffsetY - 60), buttonDimensions, "Jump Man");
            title.Label.Font = AssetManager.FetchFont("temporaryBiggest");
            Register.Add(title);

            singleplayer = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), buttonDimensions, "Singleplayer");
            singleplayer.Label.Font = AssetManager.FetchFont("temporaryBigger");
            singleplayer.Button.Pressed += delegate (object sender, EventArgs e)
            {
                if (singleplayerOverlay.IsAwake)
                    return;

                Sleep();
                singleplayerOverlay.Awake();
            };
            Register.Add(singleplayer);


            multiplayer = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 2), buttonDimensions, "Multiplayer");
            multiplayer.Label.Font = AssetManager.FetchFont("temporaryBigger");
            multiplayer.Button.Pressed += delegate (object sender, EventArgs e)
            {
                SceneManager.SwapScene("Level Select");
            };
            Register.Add(multiplayer);

            cosmetics = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 3), buttonDimensions, "Cosmetics");
            cosmetics.Label.Font = AssetManager.FetchFont("temporaryBigger");
            cosmetics.Button.Pressed += delegate (object sender, EventArgs e)
            {
                SceneManager.SwapScene("Level Select");
            };
            Register.Add(cosmetics);

            settings = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 4), buttonDimensions, "Settings");
            settings.Label.Font = AssetManager.FetchFont("temporaryBigger");
            settings.Button.Pressed += delegate (object sender, EventArgs e)
            {
                if (settingsOverlay.IsAwake)
                    return;

                Sleep();
                settingsOverlay.Awake();
            };
            Register.Add(settings);

            quit = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 5), buttonDimensions, "Quit");
            quit.Label.Font = AssetManager.FetchFont("temporaryBigger");
            quit.Button.Pressed += delegate (object sender, EventArgs e)
            {
                //0 = Exit code - OK
                Environment.Exit(0);
            };
            Register.Add(quit);
        }


    }
}
