﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Services;

namespace MagicWorld.BlendInClasses
{
    class AimingAid : DrawableGameComponent
    {
        const float transparencyFactorWithDegree = 0.4f;

        const float transparencyFactorWithoutDegree = 0.3f;

        /// <summary>
        /// milliseconds blink interval
        /// </summary>
        const double COLOR_UPDATE_CYCLE = 400;

        /// <summary>
        /// default draw color
        /// </summary>
        static readonly Color DefaultColor = Color.White;
        /// <summary>
        /// toggling blink color
        /// </summary>
        static readonly Color BlinkColor = Color.Red;

        Color currentColor = DefaultColor;

        ContentManager content;
        SpriteBatch spriteBatch;

        Texture2D arrowTex;
        Texture2D circleTex;
        Texture2D circleWithoutDegrees;

        IPlayerService playerService;
        ICameraService cameraService;
        Vector2 origin;

        public AimingAid(Game game)
            : base(game)
        {
            content = game.Content;
        }

        protected override void LoadContent()
        {
            circleTex = content.Load<Texture2D>("AimingAid/aidCircle");
            arrowTex = content.Load<Texture2D>("AimingAid/aimArrow");
            circleWithoutDegrees = content.Load<Texture2D>("AimingAid/aidCircleWithoutDegree");

            origin = new Vector2(arrowTex.Width / 2, arrowTex.Height / 2);

            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
            cameraService = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            base.LoadContent();
        }

        double colorUpdateCycle;

        public override void Update(GameTime gameTime)
        {
            if (playerService.isNearCastingCancel)
            {
                if (colorUpdateCycle <= 0)
                {
                    if (currentColor == DefaultColor)
                    {
                        currentColor = BlinkColor;
                    }
                    else
                    {
                        currentColor = DefaultColor;
                    }
                    colorUpdateCycle = COLOR_UPDATE_CYCLE;
                }
                colorUpdateCycle -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                currentColor = DefaultColor;
            }            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (playerService.isAiming)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraService.TransformationMatrix);
                spriteBatch.Draw(circleTex, playerService.Position, null, currentColor * transparencyFactorWithDegree, 0, origin, 1.0f, SpriteEffects.None, 0f);
                spriteBatch.Draw(arrowTex, playerService.Position, null, currentColor * transparencyFactorWithDegree, -(float)playerService.SpellAimAngle, origin, 1.0f, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.End();
            }
            else if (playerService.IsCasting)//push and pull disable this if, if you don not like it
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraService.TransformationMatrix);
                spriteBatch.Draw(circleWithoutDegrees, playerService.CurrentSpell.Bounds.getRectangle(), currentColor * transparencyFactorWithoutDegree);         
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
