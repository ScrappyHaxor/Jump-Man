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
        public readonly List<Background> Backgrounds;
        public readonly List<ScrapVector> TestPositions;
        public readonly List<Entity> Traps;
        public EndOfLevel EndOfLevel;

        public LevelData()
        {
            Player = new Player(ScrapVector.Zero);
            Platforms = new List<Platform>();
            Backgrounds = new List<Background>();
            TestPositions = new List<ScrapVector>();
            Traps = new List<Entity>();
            EndOfLevel = new EndOfLevel(new ScrapVector(0, -100), new ScrapVector(64, 64));
        }

        public LevelData(Player player, List<Platform> platforms, List<Background> backgrounds, List<ScrapVector> testPositions, List<Entity> traps, EndOfLevel endOfLevel)
        {
            Player = player;
            Platforms = platforms;
            Backgrounds = backgrounds;
            TestPositions = testPositions;
            Traps = traps;
            EndOfLevel = endOfLevel;
        }
    }
}
