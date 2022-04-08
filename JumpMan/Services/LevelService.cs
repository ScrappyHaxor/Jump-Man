using JumpMan.Container;
using JumpMan.Objects;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JumpMan.Services
{
    public enum DataType
    {
        PLAYER,
        PLATFORM
    }

    public static partial class LevelService
    {
        public static LevelData DeserializeLevel(string levelPath)
        {
            Player player = null;
            List<Platform> platforms = new List<Platform>();

            foreach (string rawData in File.ReadAllLines(levelPath))
            {
                string[] chunks = rawData.Split(";");
                //Each row is structured differently with an identifier at the start corresponding to the enum DATA_TYPE (referred to as objectID)
                //PLAYER: objectID(0);xPosition;yPosition
                //PLATFORM: objectID(1);textureName;xPosition;yPosition;width;height

                int rawObjectID;
                if (!int.TryParse(chunks[0], out rawObjectID))
                {
                    //Log error here
                    return default;
                }

                DataType objectID = (DataType)rawObjectID;
                if (objectID == DataType.PLAYER)
                {
                    player = new Player(new ScrapVector(int.Parse(chunks[1]), int.Parse(chunks[2])));
                }
                else if (objectID == DataType.PLATFORM)
                {
                    ScrapVector position = new ScrapVector(int.Parse(chunks[2]), int.Parse(chunks[3]));
                    ScrapVector dimensions = new ScrapVector(int.Parse(chunks[4]), int.Parse(chunks[5]));
                    platforms.Add(new Platform(chunks[1], position, dimensions));
                }

                if (player == null)
                {
                    //Log error here
                    return default;
                }
            }

            return new LevelData(player, platforms);
        }
    }
}
