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
        public PushPullElement(String texture, CollisionType collision, Level level, Vector2 position,bool enableGravity=true)
            : base (texture, collision, level, position,true,enableGravity)
        {
        }

        public override void Update(GameTime gameTime)
        {
            pushPullHandler.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// cold spell increases lifetime warm spell shortens on 10%
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public override bool SpellInfluenceAction(Spell spell)
        {
            //TODO BUG!! we must decide on which side of the wizard is the element
            if (spell.SpellType == SpellType.PullSpell)
            {
                //TODO put values into constant class
                Vector2 push = this.Position - spell.Position;
                push.Normalize();
                pushPullHandler.setXAcceleration(1.1f, 0, 2, -0.2f);
                pushPullHandler.start(this,1000, push);
                return false;
            }
            else if (spell.SpellType == SpellType.PushSpell)
            {
                //TODO put values into constant class
                Vector2 pull = spell.Position - this.Position;
                pull.Normalize();
                pushPullHandler.setXAcceleration(1f,0, 1.1f, 0.07f);                
                pushPullHandler.start(this, 1000,pull);
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }
   

        
    }
}