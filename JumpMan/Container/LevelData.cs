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

        public LevelData()
        {
            Player = new Player(ScrapVector.Zero);
            Platforms = new List<Platform>();
            Backgrounds = new List<Background>();
            TestPositions = new List<ScrapVector>();
            Traps = new List<Entity>();
        }

        public LevelData(Player player, List<Platform> platforms, List<Background> backgrounds, List<ScrapVector> testPositions, List<Entity> traps)
        {
            Player = player;
            Platforms = platforms;
            Backgrounds = backgrounds;
            TestPositions = testPositions;
            Traps = traps;
        }
    }
}
