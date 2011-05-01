﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace KidnapThePrincess
{
    class Hero : Person
    {
        private Rectangle area;

        public Rectangle Area
        {
            get { return area; }
            set { area = value; }
        }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private Vector2 dest;

        public Vector2 Destination
        {
            get { return dest; }
            set { dest = value; }
        }

        protected List<Enemy> enemies;


        Boolean freezed = false;

        public Boolean Freezed
        {
            get { return freezed; }
            set { freezed = value; }
        }

        //flag that says: hero can move during freezing
        protected Boolean canMoveFreezed = false;

        //specifies how long the attack is visualised
        private int showAttackTimeSave;
        private Boolean isAttacking = false;

        public Boolean IsAttacking
        {
            get { return isAttacking; }
            set {
                if (lastAttack + attackDelay <= lastUpdateTimeSave)
                {
                    isAttacking = value;
                    showAttackTimeSave = lastUpdateTimeSave;
                    lastAttack = showAttackTimeSave;
                }
            }
        }

        //last time this hero was updated
        private int lastUpdateTimeSave;


        //must be stored to endfreeze at some time
        private int lastFreezeTime;


        //you could specify if a hero only could attack with some delay between
        protected int attackDelay = 0;

        protected int lastAttack;


        /// <summary>
        /// Constructor for a hero
        /// </summary>
        /// <param name="tex">The texture that will represent the hero in game.</param>
        /// <param name="area">For the goblin the rectangle equals the carriage, for the others it's the playArea</param>
        public Hero(Texture2D tex, Rectangle area,List<Enemy> enemies)
            : base(tex)
        {
            this.area = area;
            this.enemies = enemies;
            isActive = false;
        }


        /// <summary>
        /// Constructor for a hero
        /// </summary>
        /// <param name="tex">The texture that will represent the hero in game.</param>
        /// <param name="area">For the goblin the rectangle equals the carriage, for the others it's the playArea</param>
        public Hero(Texture2D tex, Rectangle area)
            : base(tex)
        {
            this.area = area;
            isActive = false;
        }

        public override void Update(GameTime time)
        {
            //reset show attack flag after a moment 
            if (IsAttacking)
            {
                if(lastUpdateTimeSave > showAttackTimeSave + 70){
                    IsAttacking = false; //attack is over
                }                
                attack();
            }

            //check if an enemy collidates with the hero
            foreach (Enemy e in enemies)
            {
                if (e.IsDangerous() && GeometryHelper.Intersects(this.Bounds, e.Bounds))
                {
                    freezed = true;
                    lastFreezeTime = (int)time.TotalGameTime.TotalMilliseconds;
                    break;
                }
            }

            if (freezed)
            {
                if (lastUpdateTimeSave > lastFreezeTime + 500)
                {
                    freezed = false; //attack is over
                }
            }


            if (!freezed || canMoveFreezed) //if hero was hit by enemy he can move some time
            {
                if (isActive)
                {
                    if (area.X > Position.X) Position = new Vector2(area.X, Position.Y);
                    if (area.Right - sprite.Width < Position.X) Position = new Vector2(area.Right - sprite.Width, Position.Y);
                    if (area.Y > Position.Y) Position = new Vector2(Position.X, area.Y);
                    if (area.Bottom < Position.Y) Position = new Vector2(Position.X, area.Bottom);
                }
                else if (!isActive)
                {
                    Direction = Destination - Position;
                    Direction = Vector2.Normalize(Direction);
                }
            }
            lastUpdateTimeSave = (int)time.TotalGameTime.TotalMilliseconds;
            base.Update(time);
        }

        //save time when attack was pressed
        protected virtual void attack()
        {

        }

    }
}
