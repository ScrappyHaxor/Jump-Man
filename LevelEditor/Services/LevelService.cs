using JumpMan.Container;
using JumpMan.Objects;
using JumpMan.Services;
using ScrapBox.Framework.Math;
using System.IO;

namespace LevelEditor.Services
{
    public static partial class LevelService
    {
        public static void SerializeLevel(string name, LevelData data)
        {
            StreamWriter writer = new StreamWriter(name, false);
            writer.WriteLine($"{(int)DataType.PLAYER};{data.player.Transform.Position.X};{data.player.Transform.Position.Y}");
            foreach (Platform p in data.platforms)
            {
                ScrapVector pos = p.Transform.Position;
                ScrapVector size = p.Transform.Dimensions;
                writer.WriteLine($"{(int)DataType.PLATFORM};{p.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y}");
            }

            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }
}
