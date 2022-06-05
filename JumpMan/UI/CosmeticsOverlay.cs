using JumpMan.Container;
using JumpMan.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScrapBox.Framework.ECS;
using ScrapBox.Framework.Level;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;

namespace JumpMan.UI
{
    public class CosmeticsOverlay : EntityCollection
    {
        public const int OffsetY = -500;

        public const int ButtonXSize = 300;
        public const int ButtonYSize = 80;
        public const int ButtonYOffset = ButtonYSize + 40;

        public override List<Entity> Register { get; set; }

        public GenericLabel TitleLabel;

        public GenericLabel CurrentCosmeticLabel;
        public GenericButton NextCosmeticButton;
        public GenericButton PreviousCosmeticButton;
        public GenericButton UseCosmeticButton;

        public Texture2D PlayerSprite;

        private int textureIndex;
        public readonly static string[] TextureNameTable = { "player", "player1", "player2", "player3" };
        public readonly static string[] FancyNameTable = { "Default", "Red", "Blue", "Green" };

        private SettingsData settingsData;
        

        public CosmeticsOverlay() : base(SceneManager.CurrentScene.Stack.Fetch(3))
        {
            textureIndex = 0;

            TitleLabel = new GenericLabel(new ScrapVector(0, OffsetY - 60), new ScrapVector(ButtonXSize, ButtonYSize), "Cosmetics");
            TitleLabel.Label.Font = AssetManager.FetchFont("temporaryBiggest");
            TitleLabel.Layer = layer;
            Register.Add(TitleLabel);

            CurrentCosmeticLabel = new GenericLabel(new ScrapVector(0, OffsetY - 60 + ButtonYOffset * 3.6), ScrapVector.Zero, "Normal");
            CurrentCosmeticLabel.Label.Font = AssetManager.FetchFont("temporaryBigger");
            CurrentCosmeticLabel.Layer = layer;
            Register.Add(CurrentCosmeticLabel);

            NextCosmeticButton = new GenericButton(new ScrapVector(Player.CELL_SIZE_X * 6 * 1.5, OffsetY - 60 + ButtonYOffset * 2.5), new ScrapVector(ButtonXSize, ButtonYSize), "Next");
            NextCosmeticButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
            NextCosmeticButton.Layer = layer;
            NextCosmeticButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                textureIndex++;
                if (textureIndex > TextureNameTable.Length-1)
                    textureIndex = 0;
                else if (textureIndex < 0)
                    textureIndex = TextureNameTable.Length-1;

                if (!settingsData.CosmeticStatus[textureIndex])
                {
                    UseCosmeticButton.Label.Text = "Locked";
                    UseCosmeticButton.Button.FillColor = new Color(103, 5, 5);
                    UseCosmeticButton.Button.BorderColor = new Color(244, 93, 93);
                    UseCosmeticButton.Button.HoverColor = new Color(250, 45, 45);
                }
                else
                {
                    UseCosmeticButton.Label.Text = "Use";
                    UseCosmeticButton.Button.FillColor = new Color(5, 19, 103);
                    UseCosmeticButton.Button.BorderColor = new Color(93, 139, 244);
                    UseCosmeticButton.Button.HoverColor = new Color(45, 49, 250);
                }

                CurrentCosmeticLabel.Label.Text = FancyNameTable[textureIndex];
                PlayerSprite = AssetManager.FetchTexture(TextureNameTable[textureIndex]);
            };
            Register.Add(NextCosmeticButton);

            PreviousCosmeticButton = new GenericButton(new ScrapVector(-Player.CELL_SIZE_X * 6 * 1.5, OffsetY - 60 + ButtonYOffset * 2.5), new ScrapVector(ButtonXSize, ButtonYSize), "Previous");
            PreviousCosmeticButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
            PreviousCosmeticButton.Layer = layer;
            PreviousCosmeticButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                textureIndex--;
                if (textureIndex > TextureNameTable.Length - 1)
                    textureIndex = 0;
                else if (textureIndex < 0)
                    textureIndex = TextureNameTable.Length - 1;

                if (!settingsData.CosmeticStatus[textureIndex])
                {
                    UseCosmeticButton.Label.Text = "Locked";
                    UseCosmeticButton.Button.FillColor = new Color(103, 5, 5);
                    UseCosmeticButton.Button.BorderColor = new Color(244, 93, 93);
                    UseCosmeticButton.Button.HoverColor = new Color(250, 45, 45);
                }
                else
                {
                    UseCosmeticButton.Label.Text = "Use";
                    UseCosmeticButton.Button.FillColor = new Color(5, 19, 103);
                    UseCosmeticButton.Button.BorderColor = new Color(93, 139, 244);
                    UseCosmeticButton.Button.HoverColor = new Color(45, 49, 250);
                }

                CurrentCosmeticLabel.Label.Text = FancyNameTable[textureIndex];
                PlayerSprite = AssetManager.FetchTexture(TextureNameTable[textureIndex]);
            };
            Register.Add(PreviousCosmeticButton);

            UseCosmeticButton = new GenericButton(new ScrapVector(0, OffsetY - 60 + ButtonYOffset * 4.5), new ScrapVector(ButtonXSize, ButtonYSize), "Use");
            UseCosmeticButton.Label.Font = AssetManager.FetchFont("temporaryBigger");
            UseCosmeticButton.Layer = layer;
            UseCosmeticButton.Button.Pressed += delegate (object o, EventArgs e)
            {
                if (!settingsData.CosmeticStatus[textureIndex])
                    return;

                settingsData.CosmeticInUse = TextureNameTable[textureIndex];
                settingsData.SaveSettings();

                SceneManager.SwapScene("Main Menu");
            };
            Register.Add(UseCosmeticButton);
        }

        public override void Awake()
        {
            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
            {
                settingsData = new SettingsData();
                settingsData.SaveSettings();
            }

            int index = -1;
            for (int i = 0; i < TextureNameTable.Length; i++)
            {
                if (TextureNameTable[i] == settingsData.CosmeticInUse)
                    index = i;
            }
            textureIndex = index;

            CurrentCosmeticLabel.Label.Text = FancyNameTable[index];

            PlayerSprite = AssetManager.FetchTexture(settingsData.CosmeticInUse);
            base.Awake();
        }

        public override void Sleep()
        {
            settingsData.SaveSettings();
            base.Sleep();
        }

        public override void PostLayerRender(Camera mainCamera)
        {
            Renderer.RenderSprite(PlayerSprite, new ScrapVector(0, OffsetY - 60 + ButtonYOffset * 2.5), new ScrapVector(Player.CELL_SIZE_X * 6 * 2, Player.CELL_SIZE_Y * 1 * 2), mainCamera, null);
            base.PostLayerRender(mainCamera);
        }
    }
}
