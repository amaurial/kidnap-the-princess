﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using MagicWorld.DynamicLevelContent;
using System.Diagnostics;
using MagicWorld.Spells;
using MagicWorld.HelperClasses;
using MagicWorld.Constants;

namespace MagicWorld
{
    class MatterSpell:Spell 
    {
        private const int MATTER_EXISTENCE_TIME = 500; // time that created Matter exist

        Texture2D matterTexture;

        protected Boolean gravityIsSetOffBySpell = false;
        protected Boolean influencedByPushSpell = false;
        protected Boolean influencedByPullSpell = false;


        /// <summary>
        /// Created Tile lifetime depends on Force (spell creation time) also the life time of the spell itself(so it flies a shorter time)
        /// </summary>
        /// <param name="spriteSet"></param>
        /// <param name="_origin"></param>
        /// <param name="level"></param>
        public MatterSpell(string spriteSet, Vector2 _origin, Level level)
            : base(spriteSet, _origin, level, SpellConstantsValues.CreateMatterSpellConstants.BasicCastingCost, SpellConstantsValues.CreateMatterSpellConstants.CastingCostPerSecond, SpellType.CreateMatterSpell)
        {            
            Force = SpellConstantsValues.CreateMatterSpell_Force;
            survivalTimeMs = MATTER_EXISTENCE_TIME;
            MoveSpeed = SpellConstantsValues.CreateMatterSpell_MoveSpeed;
            currentScale = SpellConstantsValues.CreateMatterSpell_currentScale;
            accelarationChangeFactorX = SpellConstantsValues.CreateMatterSpell_accelarationChangeFactor;
            accelarationChangeFactorY = 0;
            LoadContent(spriteSet);
            this.Collision = CollisionType.Platform;
        }

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float radius = ((matterTexture.Width + matterTexture.Height)/4 * currentScale); 
                return new Bounds(position, radius);
            }
        }

        public override void LoadContent(string spriteSet)
        {
            matterTexture = level.Content.Load<Texture2D>("Sprites\\MatterSpell\\matter");

            base.LoadContent(spriteSet);
            oldBounds = this.Bounds;
        }

        Bounds oldBounds;
        bool isOnGround = false;
        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.WORKING)
            {
                if (influencedByPushSpell)
                {
                    Vector2 playerPosition = level.Player.Position;
                    Vector2 acceleration = new Vector2(SpellInfluenceValues.PullAcceleration, SpellInfluenceValues.PullAcceleration);
                    if (playerPosition.X > Position.X)
                    {
                        acceleration.X *= -1;
                    }
                    if (playerPosition.Y > Position.Y)
                    {
                        acceleration.Y *= -1;
                    }
                    level.PhysicsManager.ApplyGravity(this, acceleration, gameTime);
                    influencedByPushSpell = false;
                }
                if (influencedByPullSpell)
                {
                    Vector2 playerPosition = level.Player.Position;
                    Vector2 acceleration = new Vector2(SpellInfluenceValues.PullAcceleration, SpellInfluenceValues.PullAcceleration);
                    if (playerPosition.X < Position.X)
                    {
                        acceleration.X *= -1;
                    }
                    if (playerPosition.Y < Position.Y)
                    {
                        acceleration.Y *= -1;
                    }
                    level.PhysicsManager.ApplyGravity(this, acceleration, gameTime);
                    influencedByPullSpell = false;
                }

                if (!gravityIsSetOffBySpell && !influencedByPushSpell && !influencedByPullSpell)
                {
                    level.PhysicsManager.ApplyGravity(this, PhysicValues.DEFAULT_GRAVITY, gameTime);
                }
            }

            base.Update(gameTime);
            if (SpellState == State.WORKING)
            {
                //Debug.WriteLine("MatterVelo: " + velocity);
                level.CollisionManager.HandleGeneralCollisions(this, velocity, ref oldBounds, ref isOnGround);
            }
        }

        protected override void OnWorkingStart()
        {
            survivalTimeMs *= Force;
            Debug.WriteLine("Matter starts working TIme:" +survivalTimeMs);
            base.OnWorkingStart();
        }

        protected override void OnRemove()
        {
            base.OnRemove();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(matterTexture, Bounds.getRectangle(), Color.White);
            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// cold spell increases lifetime warm spell shortens on 10%
        /// </summary>
        /// <param name="spell"></param>
        /// <returns></returns>
        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(WarmSpell))
            {
                survivalTimeMs *= 0.7;
            }
            else if (spell.GetType() == typeof(ColdSpell))
            {
                survivalTimeMs *= 1.3;
            }
            else if (spell.GetType() == typeof(NoGravitySpell))
            {
                gravityIsSetOffBySpell = true;
                return false; //do not remove spell
            }
            else if (spell.SpellType == SpellType.PushSpell)
            {
                influencedByPushSpell = true;
                return false;
            }
            else if (spell.SpellType == SpellType.PullSpell)
            {
                influencedByPullSpell = true;
                return false;
            }
            return base.SpellInfluenceAction(spell);
        }


        public override void AddOnCreationParticles()
        {
            if (level.MatterCreationParticleSystem.CurrentParticles() < 10)
            {
                level.MatterCreationParticleSystem.AddParticles(position);
            }
        }

        public override void HandleCollision()
        {   
            //check if spells leaves the level
            HandleOutOfLevelCollision();
         }

    }
}
