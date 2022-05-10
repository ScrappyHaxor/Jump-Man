using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;
using ScrapBox.Framework.Shapes;
using Rectangle = ScrapBox.Framework.Shapes.Rectangle;

namespace ModTools.UI
{
    public class SavePopup : EntityCollection
    {
        public const double BUTTON_WIDTH = 120;
        public const double BUTTON_HEIGHT = 60;

        public const double TEXTBOX_WIDTH_OFFSET = 60;
        public const double TEXTBOX_HEIGHT = 60;

        public const double OFFSET = 20;

        public override List<Entity> Register { get; set; }

        public EditorButton SaveButton;
        public EditorLabel Instruction;
        public EditorTextbox SaveName;

        public ScrapVector Position;
        public ScrapVector Dimensions;

        public Rectangle BackRect;

        private Vector2 instructionTextSize;

        public SavePopup(ScrapVector position, ScrapVector dimensions) : base(SceneManager.CurrentScene.Stack.Fetch(DefaultLayers.UI))
        {
            Position = position;
            Dimensions = dimensions;

            BackRect = new Rectangle(Position, Dimensions);

            SaveButton = new EditorButton(new ScrapVector(position.X, position.Y + dimensions.Y / 2 - BUTTON_HEIGHT / 2 - OFFSET), new ScrapVector(BUTTON_WIDTH, BUTTON_HEIGHT), "Save");
            Register.Add(SaveButton);

            Instruction = new EditorLabel(ScrapVector.Zero, "Enter level name");
            instructionTextSize = Instruction.Label.Font.MeasureString(Instruction.Label.Text);
            Instruction.Transform.Position = new ScrapVector(position.X, position.Y - dimensions.Y / 2 + instructionTextSize.Y / 2 + OFFSET);
            Register.Add(Instruction);

            SaveName = new EditorTextbox(new ScrapVector(position.X, Instruction.Transform.Position.Y + TEXTBOX_HEIGHT / 2 + OFFSET * 2), new ScrapVector(dimensions.X - TEXTBOX_WIDTH_OFFSET, TEXTBOX_HEIGHT), "Level Name");
            Register.Add(SaveName);
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
            BackRect.Position = Position;
            BackRect.Dimensions = Dimensions;

            SaveButton.Transform.Position = new ScrapVector(Position.X, Position.Y + Dimensions.Y / 2 - BUTTON_HEIGHT / 2 - OFFSET);
            Instruction.Transform.Position = new ScrapVector(Position.X, Position.Y - Dimensions.Y / 2 + instructionTextSize.Y / 2 + OFFSET);
            SaveName.Transform.Position = new ScrapVector(Position.X, Instruction.Transform.Position.Y + TEXTBOX_HEIGHT / 2 + OFFSET * 2);

            base.PreLayerTick(dt);
        }

        public override void PostLayerTick(double dt)
        {
            base.PostLayerTick(dt);
        }

        public override void PreLayerRender(Camera mainCamera)
        {
            TriangulationService.Triangulate(BackRect.Verticies, TriangulationMethod.EAR_CLIPPING, out int[] indicies);

            Renderer.RenderPolygon(BackRect.Verticies, indicies, new Color(36, 36, 36), mainCamera);
            Renderer.RenderPolygonOutline(BackRect.Verticies, Color.White, mainCamera, null, 2);
            base.PreLayerRender(mainCamera);
        }

        public override void PostLayerRender(Camera mainCamera)
        {
            base.PostLayerRender(mainCamera);
        }
    }
}
