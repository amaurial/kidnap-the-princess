﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Spells
{
    class NoGravitySpell : Spell 
    {
        public NoGravitySpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level)
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

        protected override void OnRemove()
        {
            // Calculate tile position based on the side we are walking towards.
            //float posX = Position.X + BoundingRectangle.Width / 2 * (int)direction;
            //int x = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            //int y = (int)Math.Floor(Position.Y / Tile.Height);

            //if (level.GetTile(x, y).Texture == null)//empty tile
            //{
            //    level.Tiles[x, y] = new MatterTile("Tiles/BlockA1", level, x, y, MatterTile.DEFAULT_LIFE_TIME_MS);
            //}

            base.OnRemove();
        }


        public override void HandleCollision()
        {
            //check if spells leaves the level

            Rectangle bounds = BoundingRectangle;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + bounds.Width / 2 * (int)direction;
            int x = (int)Math.Floor(posX / Tile.Width) - (int)direction;
            int y = (int)Math.Floor(Position.Y / Tile.Height);

            if (x > level.Width || x < 0 || y > level.Height || y < 0)
            {
                SpellState = State.REMOVE;
            }
        }
    }
}
