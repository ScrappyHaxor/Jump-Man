using JumpMan.Container;
using JumpMan.Objects;
using JumpMan.Services;
using ScrapBox.Framework.Math;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModTools.Services
{
    public static partial class LevelService
    {
        public static string[] PackageData(LevelData data)
        {
            List<string> temp = new List<string>();
            temp.Add($"{(int)DataType.PLAYER};{data.Player.Transform.Position.X};{data.Player.Transform.Position.Y}");
            foreach (Platform p in data.Platforms)
            {
                ScrapVector pos = p.Transform.Position;
                ScrapVector size = p.Transform.Dimensions;
                temp.Add($"{(int)DataType.PLATFORM};{p.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)p.Sprite.Mode}");
            }

            foreach (Background b in data.Backgrounds)
            {
                ScrapVector pos = b.Transform.Position;
                ScrapVector size = b.Transform.Dimensions;
                temp.Add($"{(int)DataType.BACKGROUND};{b.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)b.Sprite.Mode}");
            }

            foreach (ScrapVector position in data.TestPositions)
            {
                temp.Add($"{(int)DataType.TEST_POSITION};{position.X};{position.Y}");
            }

            return temp.ToArray();
        }

        public static void SerializeLevel(string name, LevelData data)
        {
            StreamWriter writer = new StreamWriter(name, false);
            writer.WriteLine($"{(int)DataType.PLAYER};{data.Player.Transform.Position.X};{data.Player.Transform.Position.Y}");
            foreach (Platform p in data.Platforms)
            {
                ScrapVector pos = p.Transform.Position;
                ScrapVector size = p.Transform.Dimensions;
                writer.WriteLine($"{(int)DataType.PLATFORM};{p.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)p.Sprite.Mode}");
            }

            foreach (Background b in data.Backgrounds)
            {
                ScrapVector pos = b.Transform.Position;
                ScrapVector size = b.Transform.Dimensions;
                writer.WriteLine($"{(int)DataType.BACKGROUND};{b.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)b.Sprite.Mode}");
            }

            foreach (ScrapVector position in data.TestPositions)
            {
                writer.WriteLine($"{(int)DataType.TEST_POSITION};{position.X};{position.Y}");
            }

            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }
}
