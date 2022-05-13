using ScrapBox.Framework.Math;
using System.IO;

namespace JumpMan.Container
{
    public class SaveFile
    {
        public readonly string LevelName;
        public readonly ScrapVector Position;

        public SaveFile(string levelName, ScrapVector position)
        {
            if (levelName.Contains('.'))
                levelName = levelName.Replace(".data", string.Empty);

            LevelName = levelName;
            Position = position;
        }

        public void Save()
        {
            if (!Directory.Exists("saves"))
                Directory.CreateDirectory("saves");

            StreamWriter writer = new StreamWriter($"saves\\{LevelName}.save", false);
            writer.WriteLine($"{Position.X};{Position.Y}");
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        public static SaveFile Load(string levelName)
        {
            if (!Directory.Exists("saves"))
                Directory.CreateDirectory("saves");

            if (levelName.Contains('.'))
                levelName = levelName.Replace(".data", string.Empty);

            if (!File.Exists($"saves\\{levelName}.save"))
                return null;

            string[] lines = File.ReadAllLines($"saves\\{levelName}.save");
            if (lines.Length != 1)
                return null;

            string[] chunks = lines[0].Split(";");
            if (chunks.Length != 2)
                return null;

            if (!double.TryParse(chunks[0], out double x) || !double.TryParse(chunks[1], out double y))
                return null;

            return new SaveFile(levelName, new ScrapVector(x, y));
        }
    }
}
