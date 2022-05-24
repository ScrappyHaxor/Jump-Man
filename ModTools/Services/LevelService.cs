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

            foreach (Entity trap in data.Traps)
            {
                if (trap.GetType() == typeof(Glue))
                {
                    Glue convertedTrap = (Glue)trap;
                    temp.Add($"{(int)DataType.TRAP};{(int)TrapType.GLUE};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
                else if (trap.GetType() == typeof(Platform))
                {
                    Platform convertedTrap = (Platform)trap;
                    temp.Add($"{(int)DataType.TRAP};{(int)TrapType.ILLUSION};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
                else if (trap.GetType() == typeof(ScrollingPlatform))
                {
                    ScrollingPlatform convertedTrap = (ScrollingPlatform)trap;
                    temp.Add($"{(int)DataType.TRAP};{(int)TrapType.SCROLLING};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y};{convertedTrap.IsLeft};{convertedTrap.ScrollSpeed}");
                }
                else if (trap.GetType() == typeof(FeetBouncePlatform))
                {
                    FeetBouncePlatform convertedTrap = (FeetBouncePlatform)trap;
                    temp.Add($"{(int)DataType.TRAP};{(int)TrapType.BOUNCE};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
                else if (trap.GetType() == typeof(TeleportPlatform))
                {
                    TeleportPlatform convertedTrap = (TeleportPlatform)trap;
                    temp.Add($"{(int)DataType.TRAP};{(int)TrapType.TELEPORT};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
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

            foreach (Entity trap in data.Traps)
            {
                if (trap.GetType() == typeof(Glue))
                {
                    Glue convertedTrap = (Glue)trap;
                    writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.GLUE};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
                else if (trap.GetType() == typeof(Platform))
                {
                    Platform convertedTrap = (Platform)trap;
                    writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.ILLUSION};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
                else if (trap.GetType() == typeof(ScrollingPlatform))
                {
                    ScrollingPlatform convertedTrap = (ScrollingPlatform)trap;
                    writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.SCROLLING};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y};{convertedTrap.IsLeft};{convertedTrap.ScrollSpeed}");
                }
                else if (trap.GetType() == typeof(FeetBouncePlatform))
                {
                    FeetBouncePlatform convertedTrap = (FeetBouncePlatform)trap;
                    writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.BOUNCE};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
                else if (trap.GetType() == typeof(TeleportPlatform))
                {
                    TeleportPlatform convertedTrap = (TeleportPlatform)trap;
                    writer.WriteLine($"{(int)DataType.TRAP};{(int)TrapType.TELEPORT};{convertedTrap.Sprite.Texture.Name};{convertedTrap.Transform.Position.X};{convertedTrap.Transform.Position.Y};{convertedTrap.Transform.Dimensions.X};{convertedTrap.Transform.Dimensions.Y}");
                }
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
