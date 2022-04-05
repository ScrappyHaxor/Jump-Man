using JumpMan.Container;
using JumpMan.Objects;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JumpMan.Services
{
    public static partial class LevelService
    {
        public static LevelData DeserializeLevel(string levelPath)
        {
            Player player = null;
            List<Platform> platforms = new List<Platform>();

            foreach (string data in File.ReadAllLines(levelPath))
            {
                string[] chunks = data.Split(";");
                if (player == null)
                {
                    player = new Player(new ScrapVector(int.Parse(chunks[0]), int.Parse(chunks[1])));
                    continue;
                }

                ScrapVector position = new ScrapVector(int.Parse(chunks[1]), int.Parse(chunks[2]));
                ScrapVector dimensions = new ScrapVector(int.Parse(chunks[3]), int.Parse(chunks[4]));

                Platform p = new Platform(position, dimensions);
                p.Sprite.Texture = AssetManager.FetchTexture(chunks[0]);
                platforms.Add(p);
            }

            return new LevelData(player, platforms);
        }
    }
}
