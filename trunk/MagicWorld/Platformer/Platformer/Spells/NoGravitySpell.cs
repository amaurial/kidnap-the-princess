﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicWorld.Spells
{
    class NoGravitySpell : Spell 
    {
        private const int manaBasicCost = 200;
        private const float manaCastingCost = 1f;

        public NoGravitySpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, manaBasicCost, manaCastingCost)
        {            
            Force = 1;
            survivalTimeMs = 5000;
            MoveSpeed = 40.0f;
            LoadContent(spriteSet);
            sprite.PlayAnimation(idleAnimation);
            durationOfActionMs = 5000;
        }
        
        public override void LoadContent(string spriteSet)
        {
            // Load animations.
            spriteSet = "Sprites/" + spriteSet + "/";
            runAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Run"), 0.1f, true,3);
            idleAnimation = new Animation(level.Content.Load<Texture2D>(spriteSet + "Idle"), 0.15f, true,3);

            base.LoadContent(spriteSet);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
