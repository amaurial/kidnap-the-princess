﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.HelperClasses;
using MagicWorld.DynamicLevelContent;
using System.Collections.Generic;
using MagicWorld.Spells;
using ParticleEffects;
using System.Diagnostics;

namespace MagicWorld
{
    /// <summary>
    /// Base class that determines a spell
    /// </summary>
    public abstract class Spell : BasicGameElement
    {
        #region "mana"
            /// <summary>
            /// Basic Cost to pay if 
            /// </summary>
            public int ManaBasicCost {get{return manaBasicCost;}}
            private int manaBasicCost;
            /// <summary>
            /// Costs while casting the spell
            /// </summary>
            public  float ManaCastingCost{get{return manaCastingCost;}}
            private float manaCastingCost;
        #endregion

            public SpellType SpellType { get; protected set; }

        #region properties

        public enum State { CREATING, WORKING, REMOVE };
        private State spellState;

        public State SpellState
        {
            get { return spellState; }
            set
            {
                switch (value)
                {
                    case State.REMOVE:
                        OnRemove();
                        isRemovable = true;
                        break;
                    case State.CREATING:
                        OnCreateStart();
                        break;
                    case State.WORKING:
                        OnWorkingStart();
                        break;
                }
                spellState = value;

            }
        }

        public override Bounds Bounds
        {
            get
            {
                // Calculate bounds within texture size.
                float radius = (sprite.Animation.FrameWidth+ sprite.Animation.FrameHeight)/2 * 0.3f*currentScale;
                return new Bounds(position, radius);
            }
        }

        protected float currentScale = 1.0f;
        protected float MaxScale = 3.0f;
        protected float MoveSpeed = 0;

        /// <summary>
        /// Force of the spell.
        /// It may work as factor that scale distance,velocity or time
        /// </summary>
        private int force;
        public int Force
        {
            set { force = value; }
            get { return force; }
        }

        protected SpriteEffects flip = SpriteEffects.None;

        private float rotation = 0.0f;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// velocity of the movement of the spell
        /// </summary>
        protected Vector2 velocity;
        public Vector2 Velocity
        {
            set { velocity = value; }
            get { return velocity; }
        }

        protected Vector2 direction;

        public Vector2 Direction
        {
            set { direction = value; }
            get { return direction; }
        }

        protected float currentAccelaration = 1;
        protected float accelarationChangeFactor = 0; //will be multiplyed be elaspes seconds
        protected float accelarationMax = 2.0f;
        protected float accelarationMin = 0f;

        /// <summary>
        /// describes how long a spell is alive
        /// </summary>
        protected double survivalTimeMs;
        public double SurvivalTimeMs
        {
            set { survivalTimeMs = value; }
            get { return survivalTimeMs; }
        }

        // Animations
        protected Animation runAnimation=null;
        protected Animation idleAnimation=null;
        protected AnimationPlayer sprite;

        /// <summary>
        /// describes how long the magic of a spell influences a attacked object
        /// </summary>
        protected double durationOfActionMs = 0;

        /// <summary>
        /// duration depents on spell force
        /// </summary>
        public double DurationOfActionMs
        {
            get { return durationOfActionMs * Force; }
        }

        #endregion

        #region methods

        public Spell(string spriteSet, Vector2 position, Level level, int manaBasicCost, float manaCastingCost, SpellType spellType)
            : base(level)
        {
            SpellType = spellType;
            this.manaBasicCost = manaBasicCost;
            this.manaCastingCost = manaCastingCost;

            Velocity = Vector2.Zero;
            Force = 0;
            Position = position;
            this.level = level;
            SpellState = State.CREATING;
            LoadContent(spriteSet);
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public override void LoadContent(string spriteSet)
        {
            base.LoadContent("");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 paintPosition = Bounds.Position;
                       
            // Draw that sprite.
            if(sprite.Animation!=null)
                sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, rotation);
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (SpellState == State.WORKING)
            {
                HandleMovement(gameTime);
                HandleLiveTime(gameTime);
                //does not work proper dont know why at the moment
                //if (direction == new Vector2()) { //if spell has no direction and stay on currnt position use idle animation
                //    if (sprite.Animation != idleAnimation)
                //    {
                //        sprite.PlayAnimation(idleAnimation);
                //    }
                //}
                //else
                {
                    //only start playing if animation changes because frame position is reseted
                    if (sprite.Animation != runAnimation)
                    {
                        sprite.PlayAnimation(runAnimation);
                    }
                }
                HandleCollision();
            }
            else if (SpellState == State.CREATING)
            {
                Grow(gameTime);
                //only start playing if animation changes because frame position is reseted
                if (sprite.Animation != idleAnimation)
                {
                    sprite.PlayAnimation(idleAnimation);
                }
            }
        }

        /// <summary>
        /// Handles spell movement
        /// </summary>
        /// <param name="gameTime"></param>
        protected virtual void HandleMovement(GameTime gameTime)
        {
            //accelaration
            currentAccelaration += (float)gameTime.ElapsedGameTime.TotalSeconds * accelarationChangeFactor;
            if (currentAccelaration < accelarationMin)
                currentAccelaration = accelarationMin;
            if (currentAccelaration > accelarationMax)
                currentAccelaration = accelarationMax;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            // Move in the current direction.
            velocity = new Vector2((float)direction.X * MoveSpeed * elapsed * currentAccelaration, (float)direction.Y * MoveSpeed * elapsed * currentAccelaration);
            Position = Position + velocity;
        }

        /// <summary>
        /// Handles how long a spell lives
        /// </summary>
        /// <param name="gameTime"></param>
        private void HandleLiveTime(GameTime gameTime)
        {
            //remove a spell after his time is come
            if (survivalTimeMs < 0)
            {
                this.SpellState = State.REMOVE;
            }
            survivalTimeMs -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }


        protected virtual void OnRemove()
        {
        }

        protected virtual void OnCreateStart()
        {
        }

        protected virtual void OnWorkingStart()
        {
        }

        public virtual void Grow(GameTime gameTime)
        {
            if (currentScale <= MaxScale)
            {
                currentScale += 0.02f;
                if(idleAnimation!=null)
                    idleAnimation.Scale = currentScale;
                if(runAnimation!=null)
                    runAnimation.Scale = currentScale;
                Force++;
            }
        }

        /// <summary>
        /// adds creation particles
        /// </summary>
        public virtual void AddOnCreationParticles()
        {            
            if (level.MagicParticleSystem.CurrentParticles() < 10)
            {
                level.MagicParticleSystem.AddParticles(position);
            }
        }

        /// <summary>
        /// throw away current spell
        /// </summary>
        public virtual void FireUp()
        {
            SpellState = State.WORKING;
        }

        ///Every type of collision has its own virtual method, so its easy to override special behavior in subclasses
        #region collision

        /// <summary>
        /// handels collision with enemie
        /// </summary>
        public virtual void HandleEnemyCollision()
        {
            List<Enemy> collisionEnemies = new List<Enemy>();

            level.CollisionManager.CollidateWithEnemy(this, ref collisionEnemies);
            //enemy collision
            foreach (Enemy enemy in collisionEnemies)
            {                
                    if (enemy.SpellInfluenceAction(this))
                    {
                        SpellState = State.REMOVE;
                    }                
            }
        }

        /// <summary>
        /// handels collision with tiles
        /// </summary>
        public virtual void HandleCollisionWithLevelElements()
        {
            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();

            level.CollisionManager.CollidateWithGeneralLevelElements(this, ref collisionObjects);
            //enemy collision
            foreach (BasicGameElement tile in collisionObjects)
            {
                if (tile.SpellInfluenceAction(this))
                {
                    SpellState = State.REMOVE;
                }
            }
        }

        /// <summary>
        /// handels collision with level bounds
        /// </summary>
        public virtual void HandleOutOfLevelCollision()
        {
            if (level.CollisionManager.CollidateWithLevelBounds(this))
            {
                SpellState = State.REMOVE;
            }
        }

        /// <summary>
        /// handels collision with player
        /// </summary>
        public virtual void HandlePlayerCollision()
        {
            //check if collision is occured
            if (level.CollisionManager.CollidateWithPlayer(this))
            {
                if (level.Player.SpellInfluenceAction(this))
                {
                    SpellState = State.REMOVE;
                }
            }
        }

        /// <summary>
        /// handels collision with other objects
        /// </summary>
        public virtual void HandleObjectsCollision()
        {

            List<BasicGameElement> levelElements = new List<BasicGameElement>();

            level.CollisionManager.CollidateWithGeneralLevelElements(this, ref levelElements);
            //enemy collision
            foreach (BasicGameElement elem in levelElements)
            {
                if (elem.SpellInfluenceAction(this))
                {
                    SpellState = State.REMOVE;
                }
            }
        }

        /// <summary>
        /// handels collision with tiles and enemies and level bounds
        /// </summary>
        public virtual void HandleCollision()
        {
            //player collision
            HandlePlayerCollision();

            //enemy collision
            HandleEnemyCollision();

            //Tile collision
            HandleCollisionWithLevelElements();        

            //check if spells leaves the level
            HandleOutOfLevelCollision();

            //objects collision
            HandleObjectsCollision();
        }

        #endregion collision

        #endregion
    }
}
