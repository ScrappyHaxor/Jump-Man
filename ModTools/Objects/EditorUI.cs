using ModTools.Core;
using ModTools.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;

namespace ModTools.Objects
{
    public class EditorUI : Entity
    {
        public override string Name => "Editor UI";

        public Camera Camera;
        public EditorPlayer EditorPlayer;
        public EditorGhost EditorGhost;
        public bool SaveFlag;
        public bool InstructionFlag;

        private SpriteFont editorFontBig;
        private SpriteFont editorFontSmall;

        public EditorUI() : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {

        }

        public override void Awake()
        {
            if (Camera == null)
            {
                LogService.Log(App.AssemblyName, "EditorUI", "Awake", "Camera is null", Severity.ERROR);
                return;
            }

            if (EditorPlayer == null)
            {
                LogService.Log(App.AssemblyName, "EditorUI", "Awake", "Editor Player is null", Severity.ERROR);
                return;
            }

            if (EditorGhost == null)
            {
                LogService.Log(App.AssemblyName, "EditorUI", "Awake", "Editor Ghost is null", Severity.ERROR);
                return;
            }

            editorFontBig = AssetManager.FetchFont("editorBig");
            editorFontSmall = AssetManager.FetchFont("editorSmall");

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
            if (SaveFlag)
                return;

            Renderer.RenderLine(ScrapVector.Zero, EditorGhost.Transform.Position, Color.White, Camera, null, 3);

            Renderer.RenderText(editorFontBig, "Level Editor States", new ScrapVector(10, 10), Color.White);
            Renderer.RenderText(editorFontSmall, $"Placing: {EditorPlayer.Controller.PlacingState}", new ScrapVector(10, 35), Color.White);
            Renderer.RenderText(editorFontSmall, $"Placement Position: {EditorGhost.Transform.Position.X} {EditorGhost.Transform.Position.Y}", new ScrapVector(10, 55), Color.White);
            Renderer.RenderText(editorFontSmall, $"Camera Position: {Camera.Position.X} {Camera.Position.Y}", new ScrapVector(10, 75), Color.White);

            if (EditorPlayer.Controller.PlacingState == Placing.PLATFORMS)
            {
                string evaluation = EditorGhost.GhostPlatformFlag ? "Yes" : "No";
                Renderer.RenderText(editorFontSmall, $"Platform Size: {EditorGhost.Transform.Dimensions.X} {EditorGhost.Transform.Dimensions.Y}", new ScrapVector(10, 95), Color.White);
                Renderer.RenderText(editorFontSmall, $"Platform Texture: {EditorGhost.PlatformTextures[EditorGhost.PlatformTextureIndex]}", new ScrapVector(10, 115), Color.White);
                Renderer.RenderText(editorFontSmall, $"Platform Ghost: {evaluation}", new ScrapVector(10, 135), Color.White);

            }
            else if (EditorPlayer.Controller.PlacingState == Placing.BACKGROUNDS)
            {
                Renderer.RenderText(editorFontSmall, $"Background Size: {EditorGhost.Transform.Dimensions.X} {EditorGhost.Transform.Dimensions.Y}", new ScrapVector(10, 95), Color.White);
                Renderer.RenderText(editorFontSmall, $"Background Texture: {EditorGhost.BackgroundTextures[EditorGhost.BackgroundTextureIndex]}", new ScrapVector(10, 115), Color.White);
            }
            else if (EditorPlayer.Controller.PlacingState == Placing.MOVING_PLATFORMS)
            {
                Renderer.RenderText(editorFontSmall, $"Moving Platform Extent: {EditorGhost.Extent}", new ScrapVector(10, 95), Color.White);
                Renderer.RenderText(editorFontSmall, $"Moving Platform Step: {EditorGhost.Step}", new ScrapVector(10, 115), Color.White);
                Renderer.RenderText(editorFontSmall, $"Moving Platform Axis Flipped: {EditorGhost.AxisFlippedFlag}", new ScrapVector(10, 135), Color.White);
            }
            else if (EditorPlayer.Controller.PlacingState == Placing.SCROLLING)
            {
                string evaluation = EditorGhost.IsLeft ? "Left" : "Right";
                Renderer.RenderText(editorFontSmall, $"Scrolling Platform Direction: {evaluation}", new ScrapVector(10, 95), Color.White);
                Renderer.RenderText(editorFontSmall, $"Scrolling Platform Speed: {EditorGhost.ScrollSpeed}", new ScrapVector(10, 115), Color.White);
            }

            Renderer.RenderText(editorFontBig, "Controls", new ScrapVector(10, 165), Color.White);

            if (!InstructionFlag)
            {
                Renderer.RenderText(editorFontSmall, $"Hide Instructions - I", new ScrapVector(10, 185), Color.White);
                Renderer.RenderText(editorFontSmall, $"WASD - Move camera", new ScrapVector(10, 205), Color.White);
                Renderer.RenderText(editorFontSmall, $"Left, Right arrow - Decrease, increase width", new ScrapVector(10, 225), Color.White);
                Renderer.RenderText(editorFontSmall, $"Dowm, Up arrow - Decrease, increase height", new ScrapVector(10, 245), Color.White);
                Renderer.RenderText(editorFontSmall, $"Q - Change what you are placing", new ScrapVector(10, 265), Color.White);
                Renderer.RenderText(editorFontSmall, $"Minus, Plus - Cycle between platform textures", new ScrapVector(10, 285), Color.White);
                Renderer.RenderText(editorFontSmall, $"Left click, Right click - Place platform, remove platform", new ScrapVector(10, 305), Color.White);
                Renderer.RenderText(editorFontSmall, $"F5 - Test level", new ScrapVector(10, 325), Color.White);
                Renderer.RenderText(editorFontSmall, $"Control + S - Save level", new ScrapVector(10, 345), Color.White);
                Renderer.RenderText(editorFontSmall, $"E - Change sprite mode", new ScrapVector(10, 365), Color.White);
                Renderer.RenderText(editorFontSmall, $"M - Back to menu", new ScrapVector(10, 385), Color.White);
                Renderer.RenderText(editorFontSmall, $"Insert - Decrease trap stat 1", new ScrapVector(10, 405), Color.White);
                Renderer.RenderText(editorFontSmall, $"Home - Toggle trap mode", new ScrapVector(10, 425), Color.White);
                Renderer.RenderText(editorFontSmall, $"PgUp - Increase trap stat 1", new ScrapVector(10, 445), Color.White);
                Renderer.RenderText(editorFontSmall, $"Delete - Decrease trap stat 2", new ScrapVector(10, 465), Color.White);
                Renderer.RenderText(editorFontSmall, $"PgDown - Increase trap stat 2", new ScrapVector(10, 485), Color.White);
                Renderer.RenderText(editorFontSmall, $"End - Toggle sprite for platforms", new ScrapVector(10, 505), Color.White);
                Renderer.RenderText(editorFontSmall, $"Space - Change texture of placed object to active texture", new ScrapVector(10, 525), Color.White);
            }
            else
            {
                Renderer.RenderText(editorFontSmall, $"Show Instructions - I", new ScrapVector(10, 185), Color.White);
            }
            base.PostLayerRender(camera);
        }
    }
}
