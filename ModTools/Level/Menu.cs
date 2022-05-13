using JumpMan.UI;
using ModTools.UI;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Shapes;
using System;

namespace ModTools.Level
{
    public class Menu : Scene
    {
        public const double OffsetY = -500;

        public const double buttonWidth = 850;
        public const double buttonHeight = 100;
        public const double ButtonYOffset = buttonHeight + 40;

        private GenericLabel mainLabel;
        private GenericButton editorButton;
        private GenericButton testButton;

        public Menu(ScrapApp app) : base(app)
        {

        }

        public override void LoadAssets()
        {
            AssetManager.LoadResourceFile("menu", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            mainLabel = new GenericLabel(new ScrapVector(0, OffsetY - 60), new ScrapVector(10, 10), "Mod Tools");
            mainLabel.Label.Font = AssetManager.FetchFont("temporaryBiggest");
            mainLabel.Awake();

            editorButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), new ScrapVector(buttonWidth, buttonHeight), "Level Editor");
            editorButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
            editorButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("editor");
            };

            editorButton.Awake();

            testButton = new GenericButton(new ScrapVector(0, OffsetY + ButtonYOffset * 2), new ScrapVector(buttonWidth, buttonHeight), "Play Test");
            testButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
            testButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("test");
            };

            testButton.Awake();
            base.Load(args);
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void UnloadAssets()
        {
            base.UnloadAssets();
        }

        public override void PreStackTick(double dt)
        {
            base.PreStackTick(dt);
        }

        public override void PostStackTick(double dt)
        {
            base.PostStackTick(dt);
        }

        public override void PreStackRender()
        {
            base.PreStackRender();
        }

        public override void PostStackRender()
        {
            base.PostStackRender();
        }

    }
}
