using JumpMan.Container;
using JumpMan.Core;
using JumpMan.Objects;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JumpMan.Services
{
    public enum DataType
    {
        PLAYER,
        PLATFORM,
        BACKGROUND,
        TEST_POSITION
    }

    public static partial class LevelService
    {
        public static LevelData DeserializeLevelFromData(string[] content)
        {
            LevelData data = new LevelData();

            bool playerAssigned = false;
            int row = 1;
            foreach (string rawData in content)
            {
                string[] chunks = rawData.Split(";");
                //Each row is structured differently with an identifier at the start corresponding to the enum DATA_TYPE (referred to as objectID)
                //PLAYER: objectID(0);xPosition;yPosition
                //PLATFORM: objectID(1);textureName;xPosition;yPosition;width;height
                //BACKGROUND: objectID(2);textureName;xPosition;yPosition;width;height
                //TEST_POSITION: objectID(3);xPosition;yPosition

                int rawObjectID;
                if (!int.TryParse(chunks[0], out rawObjectID))
                {
                    LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", $"Row {row} in level file is invalid. Skipping...", Severity.WARNING);
                    continue;
                }

                if (!Enum.IsDefined(typeof(DataType), rawObjectID))
                {
                    LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", $"Row {row} in level file has out of range data indicator. Skipping...", Severity.WARNING);
                    continue;
                }

                DataType objectID = (DataType)rawObjectID;
                if (objectID == DataType.PLAYER)
                {
                    if (!int.TryParse(chunks[1], out int x) || !int.TryParse(chunks[2], out int y))
                    {
                        LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", $"Player data at row: {row} in level file is invalid. Skipping...", Severity.WARNING);
                        continue;
                    }

                    data.Player = new Player(new ScrapVector(x, y));
                    playerAssigned = true;
                }
                else if (objectID == DataType.PLATFORM)
                {
                    if (!int.TryParse(chunks[2], out int x) || !int.TryParse(chunks[3], out int y) || 
                        !int.TryParse(chunks[4], out int width) || !int.TryParse(chunks[5], out int height) ||
                        !int.TryParse(chunks[6], out int mode) || !Enum.IsDefined(typeof(SpriteMode), mode))
                    {
                        LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", $"Platform data at row: {row} in level file is invalid. Skipping...", Severity.WARNING);
                        continue;
                    }

                    Platform p = new Platform(chunks[1], new ScrapVector(x, y), new ScrapVector(width, height));
                    p.Sprite.Mode = (SpriteMode)mode;

                    data.Platforms.Add(p);
                }
                else if (objectID == DataType.BACKGROUND)
                {
                    if (!int.TryParse(chunks[2], out int x) || !int.TryParse(chunks[3], out int y) ||
                        !int.TryParse(chunks[4], out int width) || !int.TryParse(chunks[5], out int height) ||
                        !int.TryParse(chunks[6], out int mode) || !Enum.IsDefined(typeof(SpriteMode), mode))
                    {
                        LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", $"Background data at row: {row} in level file is invalid. Skipping...", Severity.WARNING);
                        continue;
                    }

                    Background b = new Background(chunks[1], new ScrapVector(x, y), new ScrapVector(width, height));
                    b.Sprite.Mode = (SpriteMode)mode;

                    data.Backgrounds.Add(b);
                }
                else if (objectID == DataType.TEST_POSITION)
                {
                    if (!int.TryParse(chunks[1], out int x) || !int.TryParse(chunks[2], out int y))
                    {
                        LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", $"Test data at row: {row} in level file is invalid. Skipping...", Severity.WARNING);
                        continue;
                    }

                    data.TestPositions.Add(new ScrapVector(x, y));
                }

                row++;
            }

            if (!playerAssigned)
            {
                //Log error here
                LogService.Log(App.AssemblyName, "LevelService", "DeserializeLevelFromData", "Player info missing from level file. Assuming default.", Severity.WARNING);
            }

            return data;
        }

        public static LevelData DeserializeLevelFromFile(string levelFile)
        {
            if (!Path.IsPathRooted(levelFile))
            {
                levelFile = $"Levels\\{levelFile}";
            }

            string[] content = File.ReadAllLines(levelFile);
            return DeserializeLevelFromData(content);
        }
    }
}
