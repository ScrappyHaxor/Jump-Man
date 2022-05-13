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
        public const double heightOffset = -400;
        public const double initialSeparationOffset = 80;
        public const double separationOffset = 40;

        public const double buttonWidth = 160;
        public const double buttonHeight = 30;


        private MenuLabel mainLabel;
        private MenuButton editorButton;
        private MenuButton testButton;

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
            mainLabel = new MenuLabel(new ScrapVector(0, heightOffset), "Jump Man - Mod Tools");
            mainLabel.Awake();

            editorButton = new MenuButton(new ScrapVector(0, heightOffset + initialSeparationOffset), new ScrapVector(buttonWidth, buttonHeight), "Level Editor");
            editorButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                SceneManager.SwapScene("editor");
            };

            editorButton.Awake();

            testButton = new MenuButton(new ScrapVector(0, heightOffset + initialSeparationOffset + separationOffset), new ScrapVector(buttonWidth, buttonHeight), "Play Test");
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
