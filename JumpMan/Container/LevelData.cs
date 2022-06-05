using JumpMan.Objects;
using ScrapBox.Framework.Math;
using System.Collections.Generic;

namespace JumpMan.Container
{
    public class LevelData
    {
        public Player Player;
        public EndOfLevel EndOfLevel;
        public readonly List<Platform> Platforms;
        public readonly List<MovingPlatform> MovingPlatforms;
        public readonly List<Background> Backgrounds;
        public readonly List<ScrapVector> TestPositions;
        public readonly List<BouncePlatform> BouncePlatforms;
        public readonly List<GluePlatform> GluePlatforms;
        public readonly List<ScrollingPlatform> ScrollingPlatforms;
        public readonly List<Platform> IllusionPlatforms;
        public readonly List<TeleportPlatform> TeleportPlatforms;
        public readonly List<CosmeticDrop> CosmeticDrops;

        public LevelData()
        {
            Player = new Player(ScrapVector.Zero);
            EndOfLevel = new EndOfLevel(new ScrapVector(0, -100), new ScrapVector(64, 64));
            Platforms = new List<Platform>();
            MovingPlatforms = new List<MovingPlatform>();
            Backgrounds = new List<Background>();
            TestPositions = new List<ScrapVector>();
            BouncePlatforms = new List<BouncePlatform>();
            GluePlatforms = new List<GluePlatform>();
            ScrollingPlatforms = new List<ScrollingPlatform>();
            IllusionPlatforms = new List<Platform>();
            TeleportPlatforms = new List<TeleportPlatform>();
            CosmeticDrops = new List<CosmeticDrop>();
        }
    }
}
