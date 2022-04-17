using JumpMan.Objects;
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

        public LevelData()
        {
            Player = new Player(ScrapVector.Zero);
            Platforms = new List<Platform>();
            Backgrounds = new List<Background>();
        }

        public LevelData(Player player, List<Platform> platforms, List<Background> backgrounds)
        {
            Player = player;
            Platforms = platforms;
            Backgrounds = backgrounds;
        }
    }
}
