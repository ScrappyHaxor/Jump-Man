using JumpMan.Container;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using System;

namespace JumpMan.ECS.Components
{
    //ECS = Entity Component System. Google can probably explain it better than i can.
    //The reason this is abstracted into a component is so we dont tie the player to the singleplayer controller
    //Later in development there will be a NetPlayerController which will handle multiplayer.

    //The ControllerSystem will handle the logic of the controller. The framework is written to be data oriented which means there are a lot of classes like these
    //Containing just data.
    public class KeyboardController : Controller
    {
        public override string Name => "Player Controller";

        private bool jumpInitiated;
        private double jumpStarted;
        public ScrapVector jumpForce;
        private bool hasBounced;
        private ScrapVector lastPosition;

        private SettingsData settingsData;

        public KeyboardController()
        {

        }

        public override void TakeInput()
        {
            settingsData = SettingsData.LoadSettings();
            if (settingsData == null)
            {
                settingsData = new SettingsData();
                settingsData.SaveSettings();
            }

            ScrapVector input = ScrapVector.Zero;
            if (InputManager.IsKeyHeld(settingsData.RightKey))
            {
                input = new ScrapVector(1, input.Y);
            }
            
            if (InputManager.IsKeyHeld(settingsData.LeftKey))
            {
                input = new ScrapVector(-1, input.Y);
            }

            if (rigidbody.Grounded())
            {
                if (SelectedLevel != null && rigidbody.Transform.Position != lastPosition)
                {
                    lastPosition = rigidbody.Transform.Position;
                    SaveFile newSave = new SaveFile(SelectedLevel, rigidbody.Transform.Position);
                    newSave.Save();
                }

                hasBounced = false;
                if (!jumpInitiated)
                {
                    if (input.X < 0)
                        sprite.SourceRectangle = new Rectangle(0, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                    else if (input.X > 0)
                        sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                    else
                        sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 4, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

                    rigidbody.AddForce(input * MoveForce);
                }
                    
            }

            if (!rigidbody.Grounded() && rigidbody.Bounce() && !hasBounced)
            {
                rigidbody.AddForce(new ScrapVector(-jumpForce.X * 0.3d, jumpForce.Y * 0.1d));
                hasBounced = true;

                SoundEffect sound = AssetManager.FetchAudio("collision");
                if (sound != null)
                {
                    try
                    {
                        //sound.Play(settingsData.EffectVolume / 100f, 0, 0);
                    }
                    catch (Exception) { }
                }
                    
            }

            if (jumpInitiated && (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d) - jumpStarted >= MaxJumpMultiplier / 2)
            {
                if (input.X < 0)
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 2, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                else if (input.X > 0)
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 3, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                else
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 5, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
            }
            else
            {
                if (input.X < 0)
                    sprite.SourceRectangle = new Rectangle(0, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                else if (input.X > 0)
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                else
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 4, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
            }

            if (InputManager.IsKeyDown(settingsData.JumpKey) && rigidbody.Grounded())
            {
                jumpInitiated = true;
                jumpStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d;
            }

            if (!InputManager.IsKeyAlreadyDown(settingsData.JumpKey) && jumpInitiated && rigidbody.Grounded())
            {
                jumpInitiated = false;
                double jumpMultiplier = (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d) - jumpStarted;
                jumpMultiplier = ScrapMath.Clamp(jumpMultiplier, MinJumpMultiplier, MaxJumpMultiplier);

                SoundEffect sound = AssetManager.FetchAudio("jumping");
                if (sound != null)
                {
                    try
                    {
                        sound.Play(settingsData.EffectVolume / 100f, 0, 0);
                    }
                    catch (Exception) { }
                }

                jumpForce = new ScrapVector(0, -JumpForce * jumpMultiplier);
                if (input != ScrapVector.Zero)
                {
                    if (input.X < 0)
                        sprite.SourceRectangle = new Rectangle(0, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                    else if (input.X > 0)
                        sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                        
                    jumpForce = ScrapMath.RotatePoint(jumpForce, ScrapMath.ToRadians(input.X * JumpDirectionalDegree));
                    rigidbody.AddForce(jumpForce);
                }
                else
                {
                    sprite.SourceRectangle = new Rectangle(sprite.SourceRectangle.Width * 4, 0, sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);
                    rigidbody.AddForce(jumpForce);
                }
            }
        }
    }
}
