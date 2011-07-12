﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;
using MagicWorld.StaticLevelContent;
using MagicWorld.Spells;
using ParticleEffects;

namespace MagicWorld.DynamicLevelContent
{
    /// <summary>
    /// basic element which could be influenced by push and pull spell
    /// gravity could also has influence
    /// </summary>
    class PushPullElement : GravityElement
    {

        protected PushPullHandler pushPullHandler = new PushPullHandler();

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="texture">texture for this object</param>
        /// <param name="collision">collision type</param>
        /// <param name="level">reference to level</param>
        /// <param name="position">startposition</param>
        /// <param name="enableGravity">true=object is influenced by gravity; false only influence by push and pull</param>
        public PushPullElement(String texture, CollisionType collision, Level level, Vector2 position, Color drawColor, bool enableGravity = true)
            : base(texture, collision, level, position, drawColor, true, enableGravity)
        {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="texture">texture for this object</param>
        /// <param name="collision">collision type</param>
        /// <param name="level">reference to level</param>
        /// <param name="position">startposition</param>
        /// <param name="enableGravity">true=object is influenced by gravity; false only influence by push and pull</param>
        public PushPullElement(String texture, CollisionType collision, Level level, Vector2 position, bool enableGravity = true)
            : this(texture, collision, level, position, Color.White, enableGravity)
        {
        }

        public override void Update(GameTime gameTime)
        {
            pushPullHandler.Update(gameTime);

            base.Update(gameTime);
        }

        int pushPullParticleCounter = 0;

        /// <summary>
        /// cold spell increases lifetime warm spell shortens on 10%
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.SpellType == SpellType.PullSpell)
            {
                if (spell.SpellState == MagicWorld.Spell.State.WORKING)
                {
                    Vector2 pull = spell.Position - this.Bounds.Center;
                    pull.Normalize();
                    pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                    pushPullHandler.setYAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                    pushPullHandler.start(this, 2000, pull);
                }
                else if (pushPullParticleCounter % 20 == 0)
                {
                    Bounds bounds = Bounds;
                    level.Game.MagicParticleSystem.AddParticles(new ParticleSetting(position + new Vector2(bounds.Width / 2, bounds.Height / 2), SpellConstantsValues.PULL_COLOR, bounds.Width));
                }
                pushPullParticleCounter++;
                return false;
            }
            else if (spell.SpellType == SpellType.PushSpell)
            {
                if (spell.SpellState == MagicWorld.Spell.State.WORKING)
                {
                    Vector2 push = this.Bounds.Center - spell.Position;
                    push.Normalize();
                    pushPullHandler.setXAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                    pushPullHandler.setYAcceleration(SpellConstantsValues.PUSHPULL_DEFAULT_START_ACCELERATION, 0, 2f, SpellConstantsValues.PUSHPULL_DEFAULT_ACCELERATION_CHANGE_FACTOR);
                    pushPullHandler.start(this, 2000, push);
                }
                else if (pushPullParticleCounter % 20 == 0)
                {
                    Bounds bounds = Bounds;
                    level.Game.PushCreationParticleSystem.AddParticles(new ParticleSetting(position + new Vector2(bounds.Width / 2, bounds.Height / 2), SpellConstantsValues.PUSH_COLOR, bounds.Width / 2));
                }
                pushPullParticleCounter++;
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }
    }
}
