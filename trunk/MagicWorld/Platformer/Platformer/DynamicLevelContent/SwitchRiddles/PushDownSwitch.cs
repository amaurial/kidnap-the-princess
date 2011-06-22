﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    class PushDownSwitch : AbstractSwitch
    {



        bool activated = false;

        public bool Activated
        {
            get { return activated; }
        }


        CollisionManager.OnCollisionWithCallback collisionCallback;

        public PushDownSwitch(String texture, Level level, Vector2 position, string id)
            :base(texture,CollisionType.Passable,level,position)
        {
            this.ID = id;
            int left = (int)Math.Round(position.X);
            int top = (int)Math.Round(position.Y);

            //the bounds are only a small rectangle at bottom middle of the texture
            int boundsWith = 30;
            int boundsHeight = 30;
            this.bounds = new Bounds(left + Width / 2 - boundsWith / 2, top + Height - boundsHeight, boundsWith, boundsHeight);

            collisionCallback = HandleCollisionWithSingleObject;
        }


        public override void Update(GameTime gameTime)
        {
            CheckForPushDown();
            base.Update(gameTime);
        }

        protected void HandleCollisionWithSingleObject(BasicGameElement element){
            if(element.GetType()==typeof(MatterSpell)){
                activated = true;
            }
        }

        /// <summary>
        /// check if switch is pushed down
        /// </summary>
        void CheckForPushDown()
        {
            if (SwitchableObjects != null)
            {
                activated = false;
                if (level.CollisionManager.CollidateWithPlayer(this))
                {
                    Activate();
                    activated = true;
                }
                else
                {
                    level.CollisionManager.HandleCollisionWithoutRestrictions(this, collisionCallback);
                    if (activated)
                    {
                        Activate();
                    }
                    else
                    {
                        Dectivate();
                    }
                }
            }
        }

    }
}