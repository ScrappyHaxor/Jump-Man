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

            foreach (Entity trap in data.Traps)
            {
                TrapType type = TrapType.GLUE;
                string texture = string.Empty;
                ScrapVector position = ScrapVector.Zero;
                ScrapVector dimensions = ScrapVector.Zero;
                if (trap.GetType() == typeof(Glue))
                {
                    type = TrapType.GLUE;
                    texture = ((Glue)trap).Sprite.Texture.Name;
                    position = ((Glue)trap).Transform.Position;
                    dimensions = ((Glue)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(IllusionPlatform))
                {
                    type = TrapType.ILLUSION;
                    texture = ((IllusionPlatform)trap).Sprite.Texture.Name;
                    position = ((IllusionPlatform)trap).Transform.Position;
                    dimensions = ((IllusionPlatform)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(KnockBackPlatformLeft))
                {
                    type = TrapType.KNOCKBACK_LEFT;
                    texture = ((KnockBackPlatformLeft)trap).Sprite.Texture.Name;
                    position = ((KnockBackPlatformLeft)trap).Transform.Position;
                    dimensions = ((KnockBackPlatformLeft)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(KnockBackPlatformRight))
                {
                    type = TrapType.KNOCKBACK_RIGHT;
                    texture = ((KnockBackPlatformRight)trap).Sprite.Texture.Name;
                    position = ((KnockBackPlatformRight)trap).Transform.Position;
                    dimensions = ((KnockBackPlatformRight)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(FeetBouncePlatform))
                {
                    type = TrapType.BOUNCE_PLATFORM;
                    texture = ((FeetBouncePlatform)trap).Sprite.Texture.Name;
                    position = ((FeetBouncePlatform)trap).Transform.Position;
                    dimensions = ((FeetBouncePlatform)trap).Transform.Dimensions;
                }

                temp.Add($"{(int)DataType.TRAP};{(int)type};{texture};{position.X};{position.Y};{dimensions.X};{dimensions.Y}");
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

            foreach (Entity trap in data.Traps)
            {
                TrapType type = TrapType.GLUE;
                string texture = string.Empty;
                ScrapVector position = ScrapVector.Zero;
                ScrapVector dimensions = ScrapVector.Zero;
                if (trap.GetType() == typeof(Glue))
                {
                    type = TrapType.GLUE;
                    texture = ((Glue)trap).Sprite.Texture.Name;
                    position = ((Glue)trap).Transform.Position;
                    dimensions = ((Glue)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(IllusionPlatform))
                {
                    type = TrapType.ILLUSION;
                    texture = ((IllusionPlatform)trap).Sprite.Texture.Name;
                    position = ((IllusionPlatform)trap).Transform.Position;
                    dimensions = ((IllusionPlatform)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(KnockBackPlatformLeft))
                {
                    type = TrapType.KNOCKBACK_LEFT;
                    texture = ((KnockBackPlatformLeft)trap).Sprite.Texture.Name;
                    position = ((KnockBackPlatformLeft)trap).Transform.Position;
                    dimensions = ((KnockBackPlatformLeft)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(KnockBackPlatformRight))
                {
                    type = TrapType.KNOCKBACK_RIGHT;
                    texture = ((KnockBackPlatformRight)trap).Sprite.Texture.Name;
                    position = ((KnockBackPlatformRight)trap).Transform.Position;
                    dimensions = ((KnockBackPlatformRight)trap).Transform.Dimensions;
                }
                else if (trap.GetType() == typeof(FeetBouncePlatform))
                {
                    type = TrapType.BOUNCE_PLATFORM;
                    texture = ((FeetBouncePlatform)trap).Sprite.Texture.Name;
                    position = ((FeetBouncePlatform)trap).Transform.Position;
                    dimensions = ((FeetBouncePlatform)trap).Transform.Dimensions;
                }

                writer.WriteLine($"{(int)DataType.TRAP};{(int)type};{texture};{position.X};{position.Y};{dimensions.X};{dimensions.Y}");
            }

            writer.WriteLine($"{(int)DataType.LEVEL_END};{data.EndOfLevel.Transform.Position.X};{data.EndOfLevel.Transform.Position.Y};{data.EndOfLevel.Transform.Dimensions.X};{data.EndOfLevel.Transform.Dimensions.Y}");

            writer.Flush();
            writer.Dispose();
            writer.Close();
        }
    }
}
