using System;
using System.Collections.Generic;
using System.Text;
using JumpMan.UI;
using Microsoft.Xna.Framework;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using ScrapBox.Framework.Shapes;

namespace JumpMan.Level
{
    public class MainMenu : Scene
    {
        public const int OffsetY = -200;

        public const int ButtonYSize = 30;
        public const int ButtonYOffset = ButtonYSize + 10;

        private MainMenuButton singleplayer;
        private MainMenuButton multiplayer;
        private MainMenuButton cosmetics;
        private MainMenuButton quit;

        private Ellipse ellipse;
        private ScrapBox.Framework.Shapes.Rectangle rect;

        private Triangle triangle;

        private Polygon complex;

        public MainMenu(ScrapApp app)
            : base(app)
        {

        }

        public override void Initialize()
        {
            //Edit things like window size here
            base.Initialize();
        }

        public override void LoadAssets()
        {
            //Load assets using AssetManager

            AssetManager.LoadResourceFile("menu", Parent.Content);
            //AssetManager.LoadResourceFile("assets", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            //Instance level things

            ellipse = new Ellipse(new ScrapVector(300, 300), new ScrapVector(100, 50), 30);
            //ellipse.Rotate(30);

            rect = new ScrapBox.Framework.Shapes.Rectangle(new ScrapVector(300, 300), new ScrapVector(200, 50));
            //rect.Rotate(45);

            triangle = new Triangle(new ScrapVector(300, 300), new ScrapVector(50, 50));

            Renderer.ClearColor = new Color(10, 10, 10);

            ScrapVector buttonDimensions = new ScrapVector(120, ButtonYSize);

            singleplayer = new MainMenuButton(new ScrapVector(0, OffsetY), buttonDimensions, "Singleplayer");
            singleplayer.Button.Pressed += delegate(object sender, EventArgs e)
            {
                SceneManager.SwapScene("Level Select");
            };

            singleplayer.Awake();

            multiplayer = new MainMenuButton(new ScrapVector(0, OffsetY + ButtonYOffset * 1), buttonDimensions, "Multiplayer");
            multiplayer.Button.Pressed += delegate (object sender, EventArgs e)
            {
                SceneManager.SwapScene("Level Select");
            };

            multiplayer.Awake();

            cosmetics = new MainMenuButton(new ScrapVector(0, OffsetY + ButtonYOffset * 2), buttonDimensions, "Cosmetics");
            cosmetics.Button.Pressed += delegate (object sender, EventArgs e)
            {
                SceneManager.SwapScene("Level Select");
            };

            cosmetics.Awake();

            quit = new MainMenuButton(new ScrapVector(0, OffsetY + ButtonYOffset * 3), buttonDimensions, "Quit");
            quit.Button.Pressed += delegate (object sender, EventArgs e)
            {
                //0 = Exit code - OK
                Environment.Exit(0);
            };

            quit.Awake();

            base.Load(args);
        }

        public override void Unload()
        {
            //Destroy everything instanced in Load
            base.Unload();
        }

        public override void UnloadAssets()
        {
            //You dont really need to do anything here, my framework unloads most things automatically
            //The exception would be if you didnt use the AssetManager then assets would be unloaded here.
            base.UnloadAssets();
        }

        public override void PreStackTick(double dt)
        {
            //Works like the standard monogame update, is called before any components
            base.PreStackTick(dt);
        }

        public override void PostStackTick(double dt)
        {
            base.PostStackTick(dt);
        }

        public override void PreStackRender()
        {
            //My framework will automatically draw sprites and other components. This method is mainly just for HUD.
            base.PreStackRender();
        }

        public override void PostStackRender()
        {
            //Renderer.RenderPolygonOutline(ellipse.Verticies, Color.Purple, MainCamera);
            //Renderer.RenderPolygonOutline(rect.Verticies, Color.Orange, MainCamera);
            //Renderer.RenderPolygonOutline(triangle.Verticies, Color.Blue, MainCamera);
            //Renderer.RenderOutlineBox(new ScrapVector(300, 300), new ScrapVector(50, 50), 0, Color.White, MainCamera);

            //TriangulationService.Triangulate(rect.Verticies, TriangulationMethod.EAR_CLIPPING, out int[] indicies);
            //Renderer.RenderPolygon(rect.Verticies, indicies, Color.Purple, Color.Green, MainCamera);
            //Renderer.RenderPolygonWireframe(rect.Verticies, indicies, Color.Purple, MainCamera);

            base.PostStackRender();
        }
    }
}
