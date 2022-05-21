using ScrapBox.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JumpMan.Container
{
    public class SettingsData
    {
        public float MusicVolume;
        public float EffectVolume;

        public Keys LeftKey;
        public Keys RightKey;
        public Keys JumpKey;

        public SettingsData()
        {
            MusicVolume = 100;
            EffectVolume = 100;

            LeftKey = Keys.A;
            RightKey = Keys.D;
            JumpKey = Keys.Space;
        }

        public void SaveSettings()
        {
            StreamWriter writer = new StreamWriter($"settings.config", false);
            writer.WriteLine($"{MusicVolume}");
            writer.WriteLine($"{EffectVolume}");
            writer.WriteLine($"{(int)LeftKey}");
            writer.WriteLine($"{(int)RightKey}");
            writer.WriteLine($"{(int)JumpKey}");
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }

        public static SettingsData LoadSettings()
        {
            SettingsData settings = new SettingsData();

            if (!File.Exists($"settings.config"))
                return null;

            string[] lines = File.ReadAllLines($"settings.config");
            if (lines.Length != 5)
                return null;

            if (!float.TryParse(lines[0], out float musicVolume) || !float.TryParse(lines[1], out float effectVolume) ||
                !int.TryParse(lines[2], out int left) || !Enum.IsDefined(typeof(Keys), left) || 
                !int.TryParse(lines[3], out int right) || !Enum.IsDefined(typeof(Keys), right) ||
                !int.TryParse(lines[4], out int jump) || !Enum.IsDefined(typeof(Keys), jump))
                return null;

            settings.MusicVolume = musicVolume;
            settings.EffectVolume = effectVolume;

            settings.MusicVolume = Math.Clamp(settings.MusicVolume, 0, 100);
            settings.EffectVolume = Math.Clamp(settings.EffectVolume, 0, 100);

            settings.LeftKey = (Keys)left;
            settings.RightKey = (Keys)right;
            settings.JumpKey = (Keys)jump;

            return settings;
        }
    }
}
