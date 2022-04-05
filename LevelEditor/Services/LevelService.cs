using JumpMan.Container;
using JumpMan.Objects;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LevelEditor.Services
{
    public static partial class LevelService
    {
        public static void SerializeLevel(string name, LevelData data)
        {
            StreamWriter writer = new StreamWriter(name, false);
            writer.WriteLine($"{data.player.Transform.Position.X};{data.player.Transform.Position.Y}");
            foreach (Platform p in data.platforms)
            {
                ScrapVector pos = p.Transform.Position;
                ScrapVector size = p.Transform.Dimensions;
                writer.WriteLine($"placeholder;{pos.X};{pos.Y};{size.X};{size.Y}");
            }

            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }
}
