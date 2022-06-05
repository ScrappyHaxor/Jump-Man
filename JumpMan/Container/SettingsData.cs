using ScrapBox.Framework.Input;
using System;
using System.IO;

namespace JumpMan.Container
{
    public class SettingsData
    {
        public float MusicVolume;
        public float EffectVolume;

        public Keys LeftKey;
        public Keys RightKey;
        public Keys JumpKey;

        public Keys CameraLeftKey;
        public Keys CameraRightKey;
        public Keys CameraUpKey;
        public Keys CameraDownKey;

        public Keys CycleTextureLeft;
        public Keys CycleTextureRight;

        public const int SETTINGS_COUNT = 12;
        public const int COSMETIC_COUNT = 4;
        public bool[] CosmeticStatus;
        public string CosmeticInUse;

        public SettingsData()
        {
            MusicVolume = 100;
            EffectVolume = 100;

            LeftKey = Keys.A;
            RightKey = Keys.D;
            JumpKey = Keys.Space;

            CameraLeftKey = Keys.A;
            CameraRightKey = Keys.D;
            CameraUpKey = Keys.W;
            CameraDownKey = Keys.S;

            CycleTextureLeft = Keys.OemMinus;
            CycleTextureRight = Keys.OemPlus;

            CosmeticStatus = new bool[COSMETIC_COUNT];
            CosmeticStatus[0] = true;

            CosmeticInUse = "player";
        }

        public void SaveSettings()
        {
            StreamWriter writer = new StreamWriter($"settings.config", false);
            writer.WriteLine($"{MusicVolume}");
            writer.WriteLine($"{EffectVolume}");
            writer.WriteLine($"{(int)LeftKey}");
            writer.WriteLine($"{(int)RightKey}");
            writer.WriteLine($"{(int)JumpKey}");
            writer.WriteLine($"{(int)CameraLeftKey}");
            writer.WriteLine($"{(int)CameraRightKey}");
            writer.WriteLine($"{(int)CameraUpKey}");
            writer.WriteLine($"{(int)CameraDownKey}");
            writer.WriteLine($"{(int)CycleTextureLeft}");
            writer.WriteLine($"{(int)CycleTextureRight}");
            writer.WriteLine($"{CosmeticInUse}");

            for (int i = 0; i < CosmeticStatus.Length; i++)
            {
                writer.WriteLine($"{CosmeticStatus[i]}");
            }

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
            if (lines.Length != SETTINGS_COUNT + COSMETIC_COUNT)
                return null;

            if (!float.TryParse(lines[0], out float musicVolume) || !float.TryParse(lines[1], out float effectVolume) ||
                !int.TryParse(lines[2], out int left) || !Enum.IsDefined(typeof(Keys), left) || 
                !int.TryParse(lines[3], out int right) || !Enum.IsDefined(typeof(Keys), right) ||
                !int.TryParse(lines[4], out int jump) || !Enum.IsDefined(typeof(Keys), jump) ||
                !int.TryParse(lines[5], out int leftCamera) || !Enum.IsDefined(typeof(Keys), leftCamera) ||
                !int.TryParse(lines[6], out int rightCamera) || !Enum.IsDefined(typeof(Keys), rightCamera) ||
                !int.TryParse(lines[7], out int upCamera) || !Enum.IsDefined(typeof(Keys), upCamera) ||
                !int.TryParse(lines[8], out int downCamera) || !Enum.IsDefined(typeof(Keys), downCamera) ||
                !int.TryParse(lines[9], out int cycleLeft) || !Enum.IsDefined(typeof(Keys), cycleLeft) ||
                !int.TryParse(lines[10], out int cycleRight) || !Enum.IsDefined(typeof(Keys), cycleRight))
                return null;

            settings.MusicVolume = musicVolume;
            settings.EffectVolume = effectVolume;

            settings.MusicVolume = Math.Clamp(settings.MusicVolume, 0, 100);
            settings.EffectVolume = Math.Clamp(settings.EffectVolume, 0, 100);

            settings.LeftKey = (Keys)left;
            settings.RightKey = (Keys)right;
            settings.JumpKey = (Keys)jump;

            settings.CameraLeftKey = (Keys)leftCamera;
            settings.CameraRightKey = (Keys)rightCamera;
            settings.CameraUpKey = (Keys)upCamera;
            settings.CameraDownKey = (Keys)downCamera;

            settings.CycleTextureLeft = (Keys)cycleLeft;
            settings.CycleTextureRight = (Keys)cycleRight;

            settings.CosmeticInUse = lines[11];

            for (int i = 0; i < COSMETIC_COUNT; i++)
            {
                if (!bool.TryParse(lines[SETTINGS_COUNT + i], out bool status))
                    return null;

                settings.CosmeticStatus[i] = status;
            }

            return settings;
        }
    }
}
