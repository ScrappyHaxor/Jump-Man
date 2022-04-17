using JumpMan.Container;
using JumpMan.ECS.Systems;
using JumpMan.Objects;
using LevelEditor.Objects;
using LevelEditor.UI;
using ScrapBox.Framework;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;

namespace LevelEditor.Level
{


    public class Editor : Scene
    {
        private LevelData data;

        private EditorPlayer editorPlayer;
        private EditorGhost editorGhost;
        private EditorUI editorUI;

        public Editor(ScrapApp app) : base(app)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            ControllerSystem controllerSystem = new ControllerSystem();
            Stack.Fetch(DefaultLayers.FOREGROUND).RegisterSystem(controllerSystem);
        }

        public override void LoadAssets()
        {
            //AssetManager.LoadResourceFile("assets", Parent.Content);
            AssetManager.LoadFont("editorBig", Parent.Content);
            AssetManager.LoadFont("editorSmall", Parent.Content);
            AssetManager.LoadFont("editorButton", Parent.Content);
            AssetManager.LoadTexture("player", Parent.Content);
            AssetManager.LoadTexture("placeholder", Parent.Content);
            AssetManager.LoadTexture("placeholder2", Parent.Content);
            base.LoadAssets();
        }

        public override void Load(params object[] args)
        {
            MainCamera.Zoom = 0.5;

            data = new LevelData();
            if (args.Length == 1)
            {
                if (args[0].GetType() == typeof(string))
                {
                    data = JumpMan.Services.LevelService.DeserializeLevelFromFile(args[0].ToString());
                    data.Player.RigidBody.IsStatic = true;
                    data.Player.Awake();
                }
                else if (args[0].GetType() == typeof(string[]))
                {
                    string[] package = (string[])args[0];
                    data = JumpMan.Services.LevelService.DeserializeLevelFromData(package);
                    data.Player.RigidBody.IsStatic = true;
                    data.Player.Awake();
                }
            }

            foreach (Platform p in data.Platforms)
            {
                p.Awake();
            }

            foreach (Background b in data.Backgrounds)
            {
                b.Awake();
            }

            editorGhost = new EditorGhost();
            editorGhost.Data = data;
            editorGhost.Awake();

            editorPlayer = new EditorPlayer();
            editorPlayer.Controller.Data = data;
            editorPlayer.Controller.EditorGhost = editorGhost;
            editorPlayer.Controller.SavePopup = new SavePopup(ScrapVector.Zero, new ScrapVector(600, 250));
            editorPlayer.Awake();

            editorUI = new EditorUI();
            editorUI.Camera = MainCamera;
            editorUI.EditorGhost = editorGhost;
            editorUI.EditorPlayer = editorPlayer;
            editorUI.Awake();

            base.Load(args);
        }

        public override void UnloadAssets()
        {
            base.UnloadAssets();
        }

        public override void Unload()
        {
            base.Unload();
        }

        public override void PreStackTick(double dt)
        {
            editorUI.SaveFlag = editorPlayer.Controller.SaveFlag;
            editorUI.InstructionFlag = editorPlayer.Controller.InstructionFlag;
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
