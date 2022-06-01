using JumpMan.Container;
using JumpMan.Objects;
using JumpMan.Services;
using ScrapBox.Framework.ECS;
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
                temp.Add($"{(int)DataType.PLATFORM};{p.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)p.Sprite.Mode};{p.IsGhost}");
            }

            foreach (MovingPlatform movingPlatform in data.MovingPlatforms)
            {
                ScrapVector pos = movingPlatform.Transform.Position;
                ScrapVector size = movingPlatform.Transform.Dimensions;
                temp.Add($"{(int)DataType.MOVING_PLATFORM};{movingPlatform.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)movingPlatform.Sprite.Mode};{movingPlatform.Extent};{movingPlatform.Step};{movingPlatform.AxisFlippedFlag}");
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

            foreach (GluePlatform gluePlatform in data.GluePlatforms)
            {
                temp.Add($"{(int)DataType.TRAP};{(int)TrapType.GLUE};{gluePlatform.Sprite.Texture.Name};{gluePlatform.Transform.Position.X};{gluePlatform.Transform.Position.Y};{gluePlatform.Transform.Dimensions.X};{gluePlatform.Transform.Dimensions.Y}");
            }

            foreach (Platform illusionPlatform in data.IllusionPlatforms)
            {
                temp.Add($"{(int)DataType.TRAP};{(int)TrapType.ILLUSION};{illusionPlatform.Sprite.Texture.Name};{illusionPlatform.Transform.Position.X};{illusionPlatform.Transform.Position.Y};{illusionPlatform.Transform.Dimensions.X};{illusionPlatform.Transform.Dimensions.Y}");
            }

            foreach (ScrollingPlatform scrollingPlatform in data.ScrollingPlatforms)
            {
                temp.Add($"{(int)DataType.TRAP};{(int)TrapType.SCROLLING};{scrollingPlatform.Sprite.Texture.Name};{scrollingPlatform.Transform.Position.X};{scrollingPlatform.Transform.Position.Y};{scrollingPlatform.Transform.Dimensions.X};{scrollingPlatform.Transform.Dimensions.Y};{scrollingPlatform.IsLeft};{scrollingPlatform.ScrollSpeed}");
            }

            foreach (BouncePlatform bouncePlatform in data.BouncePlatforms)
            {
                temp.Add($"{(int)DataType.TRAP};{(int)TrapType.BOUNCE};{bouncePlatform.Sprite.Texture.Name};{bouncePlatform.Transform.Position.X};{bouncePlatform.Transform.Position.Y};{bouncePlatform.Transform.Dimensions.X};{bouncePlatform.Transform.Dimensions.Y}");
            }

            foreach (TeleportPlatform teleportPlatform in data.TeleportPlatforms)
            {
                temp.Add($"{(int)DataType.TRAP};{(int)TrapType.TELEPORT};{teleportPlatform.Sprite.Texture.Name};{teleportPlatform.Transform.Position.X};{teleportPlatform.Transform.Position.Y};{teleportPlatform.Transform.Dimensions.X};{teleportPlatform.Transform.Dimensions.Y}");
            }

            foreach (CosmeticDrop drop in data.CosmeticDrops)
            {
                temp.Add($"{(int)DataType.COSMETIC};{drop.Transform.Position.X};{drop.Transform.Position.Y};{drop.Sprite.Texture.Name}");
            }

            temp.Add($"{(int)DataType.LEVEL_END};{data.EndOfLevel.Transform.Position.X};{data.EndOfLevel.Transform.Position.Y};{data.EndOfLevel.Transform.Dimensions.X};{data.EndOfLevel.Transform.Dimensions.Y}");

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
                writer.WriteLine($"{(int)DataType.PLATFORM};{p.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)p.Sprite.Mode};{p.IsGhost}");
            }

            foreach (MovingPlatform movingPlatform in data.MovingPlatforms)
            {
                ScrapVector pos = movingPlatform.Transform.Position;
                ScrapVector size = movingPlatform.Transform.Dimensions;
                writer.WriteLine($"{(int)DataType.MOVING_PLATFORM};{movingPlatform.Sprite.Texture.Name};{pos.X};{pos.Y};{size.X};{size.Y};{(int)movingPlatform.Sprite.Mode};{movingPlatform.Extent};{movingPlatform.Step};{movingPlatform.AxisFlippedFlag}");
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

            foreach (GluePlatform gluePlatform in data.GluePlatforms)
            {
                writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.GLUE};{gluePlatform.Sprite.Texture.Name};{gluePlatform.Transform.Position.X};{gluePlatform.Transform.Position.Y};{gluePlatform.Transform.Dimensions.X};{gluePlatform.Transform.Dimensions.Y}");
            }

            foreach (Platform illusionPlatform in data.IllusionPlatforms)
            {
                writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.ILLUSION};{illusionPlatform.Sprite.Texture.Name};{illusionPlatform.Transform.Position.X};{illusionPlatform.Transform.Position.Y};{illusionPlatform.Transform.Dimensions.X};{illusionPlatform.Transform.Dimensions.Y}");
            }

            foreach (ScrollingPlatform scrollingPlatform in data.ScrollingPlatforms)
            {
                writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.SCROLLING};{scrollingPlatform.Sprite.Texture.Name};{scrollingPlatform.Transform.Position.X};{scrollingPlatform.Transform.Position.Y};{scrollingPlatform.Transform.Dimensions.X};{scrollingPlatform.Transform.Dimensions.Y};{scrollingPlatform.IsLeft};{scrollingPlatform.ScrollSpeed}");
            }

            foreach (BouncePlatform bouncePlatform in data.BouncePlatforms)
            {
                writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.BOUNCE};{bouncePlatform.Sprite.Texture.Name};{bouncePlatform.Transform.Position.X};{bouncePlatform.Transform.Position.Y};{bouncePlatform.Transform.Dimensions.X};{bouncePlatform.Transform.Dimensions.Y}");
            }

            foreach (TeleportPlatform teleportPlatform in data.TeleportPlatforms)
            {
                writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.TELEPORT};{teleportPlatform.Sprite.Texture.Name};{teleportPlatform.Transform.Position.X};{teleportPlatform.Transform.Position.Y};{teleportPlatform.Transform.Dimensions.X};{teleportPlatform.Transform.Dimensions.Y}");
            }

            foreach (CosmeticDrop drop in data.CosmeticDrops)
            {
                writer.WriteLine($"{(int)DataType.COSMETIC};{drop.Transform.Position.X};{drop.Transform.Position.Y};{drop.Sprite.Texture.Name}");
            }

            writer.WriteLine($"{(int)DataType.LEVEL_END};{data.EndOfLevel.Transform.Position.X};{data.EndOfLevel.Transform.Position.Y};{data.EndOfLevel.Transform.Dimensions.X};{data.EndOfLevel.Transform.Dimensions.Y}");

            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }
}
