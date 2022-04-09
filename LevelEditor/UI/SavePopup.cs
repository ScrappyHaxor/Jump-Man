using Microsoft.Xna.Framework;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.UI
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

        public SavePopup(ScrapVector position, ScrapVector dimensions)
        {
            Position = position;
            Dimensions = dimensions;

            SaveButton = new EditorButton(new ScrapVector(position.X, position.Y + dimensions.Y / 2 - BUTTON_HEIGHT / 2 - OFFSET), new ScrapVector(BUTTON_WIDTH, BUTTON_HEIGHT), "Save");
            Register.Add(SaveButton);

            Instruction = new EditorLabel(ScrapVector.Zero, "Enter level name");
            Vector2 textSize = Instruction.Label.Font.MeasureString(Instruction.Label.Text);
            Instruction.Transform.Position = new ScrapVector(position.X, position.Y - dimensions.Y / 2 + textSize.Y / 2 + OFFSET);
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

        public override void Update(double dt)
        {
            base.Update(dt);
        }

        public override void Draw(Camera mainCamera)
        {
            Renderer.RenderBox(Position, Dimensions, 0, new Color(46, 46, 46), mainCamera);
            Renderer.RenderOutlineBox(Position, Dimensions, 0, Color.White, mainCamera, null, 2);
            base.Draw(mainCamera);
        }
    }
}
