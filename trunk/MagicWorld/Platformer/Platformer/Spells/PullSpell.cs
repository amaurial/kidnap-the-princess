﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;

namespace MagicWorld.Spells
{
    public class PullSpell : Spell
    {

        public override Bounds Bounds
        {
            get
            {
                float radius = 0;
                if (sprite.Animation != null)
                {
                    // Calculate bounds within texture size.
                    radius = (sprite.Animation.FrameWidth) / 2 * 1f * currentScale;
                }
                return new Bounds(position, radius);
            }
        }


        public PullSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.PullSpellConstants.BasicCastingCost, SpellConstantsValues.PullSpellConstants.CastingCostPerSecond, SpellType.PullSpell)
        {
            survivalTimeMs = SpellConstantsValues.PullSpell_survivalTimeMs;
            MoveSpeed = 0;
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = SpellConstantsValues.PullSpell_durationOfActionMs;
            base.Position = level.Player.Position;
            MaxScale = SpellConstantsValues.PullSpell_MaxSize;
            growFactor = SpellConstantsValues.PullSpell_GrowRate;
            currentScale = ManaBasicCost * growFactor;            
        }
        
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/PullSpell/";
            runAnimation = new Animation("Content/Sprites/PullSpell/PullSpriteSheet", 0.1f, 10, level.Content.Load<Texture2D>(spriteSet + "PullSpriteSheet"), 0);
            runAnimation.Opacity = 1f;
            idleAnimation = runAnimation;

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.CREATING)
            {
                base.Position = level.Player.Position;

                //only start playing if animation changes because frame position is reseted
                if (sprite.Animation != idleAnimation)
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }            
        }

        /// <summary>
        /// only check collision after casting is over then remove spell
        /// </summary>
        protected override void OnWorkingStart()
        {
            level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
            SpellState = State.REMOVE;
        }

    }
}
