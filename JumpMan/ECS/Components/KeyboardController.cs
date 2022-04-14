﻿using ScrapBox.Framework.ECS;
using ScrapBox.Framework.ECS.Components;
using ScrapBox.Framework.ECS.Systems;
using ScrapBox.Framework.Input;
using ScrapBox.Framework.Managers;
using ScrapBox.Framework.Math;
using ScrapBox.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;

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

        bool jumpInitiated;
        double jumpStarted;

        public KeyboardController()
        {

        }

        public override void TakeInput()
        {
            //Do input mapping here using InputManager

            ScrapVector input = ScrapVector.Zero;
            if (InputManager.IsKeyHeld(Keys.D))
            {
                input = new ScrapVector(1, input.Y);
            }
            
            if (InputManager.IsKeyHeld(Keys.A))
            {
                input = new ScrapVector(-1, input.Y);
            }

            if (InputManager.IsKeyDown(Keys.Space) && rigidbody.Grounded())
            {
                jumpInitiated = true;
                jumpStarted = DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d;
            }

            if (!InputManager.IsKeyAlreadyDown(Keys.Space) && jumpInitiated && rigidbody.Grounded())
            {
                jumpInitiated = false;
                double jumpMultiplier = (DateTimeOffset.Now.ToUnixTimeMilliseconds() / 1000d) - jumpStarted;
                jumpMultiplier = ScrapMath.Clamp(jumpMultiplier, MIN_JUMP_MULTIPLIER, MAX_JUMP_MULTIPLIER);

                ScrapVector jumpForce = new ScrapVector(0, -JUMP_FORCE * jumpMultiplier);
                if (input != ScrapVector.Zero)
                {
                    //Cant get this to work
                    jumpForce = ScrapMath.RotatePoint(jumpForce, ScrapMath.ToRadians(input.X * JUMP_DIRECTIONAL_DEGREE));
                    rigidbody.AddForce(jumpForce);
                }
                else
                {
                    rigidbody.AddForce(jumpForce);
                }


                LogService.Out($"Jump multiplier: {jumpMultiplier}");
            }

            if (rigidbody.Grounded() && !jumpInitiated)
            {
                rigidbody.AddForce(input * MOVE_FORCE);
            }
            
        }
    }
}
