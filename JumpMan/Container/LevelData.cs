using JumpMan.Objects;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.Container
{
    public class LevelData
    {
        public Player Player;
        public readonly List<Platform> Platforms;
        public readonly List<MovingPlatform> MovingPlatforms;
        public readonly List<Background> Backgrounds;
        public readonly List<ScrapVector> TestPositions;
        public readonly List<Entity> Traps;
        public EndOfLevel EndOfLevel;
        public readonly List<CosmeticDrop> CosmeticDrops;

        public LevelData()
        {
            Player = new Player(ScrapVector.Zero);
            Platforms = new List<Platform>();
            MovingPlatforms = new List<MovingPlatform>();
            Backgrounds = new List<Background>();
            TestPositions = new List<ScrapVector>();
            Traps = new List<Entity>();
            EndOfLevel = new EndOfLevel(new ScrapVector(0, -100), new ScrapVector(64, 64));
            CosmeticDrops = new List<CosmeticDrop>();
        }

        public LevelData(Player player, List<Platform> platforms, List<MovingPlatform> movingPlatforms, List<Background> backgrounds, List<ScrapVector> testPositions, List<Entity> traps, EndOfLevel endOfLevel, List<CosmeticDrop> cosmeticDrops)
        {
            Player = player;
            Platforms = platforms;
            MovingPlatforms = movingPlatforms;
            Backgrounds = backgrounds;
            TestPositions = testPositions;
            Traps = traps;
            EndOfLevel = endOfLevel;
            CosmeticDrops = cosmeticDrops;
        }
    }
}
