using JumpMan.Objects;
using ScrapBox.Framework.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace JumpMan.Container
{
    public class LevelData
    {
        public readonly Player player;
        public readonly List<Platform> platforms;

        public LevelData()
        {
            player = new Player(ScrapVector.Zero);
            platforms = new List<Platform>();
        }

        public LevelData(Player player, List<Platform> platforms)
        {
            this.player = player;
            this.platforms = platforms;
        }
    }
}
