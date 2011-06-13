﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.DynamicLevelContent;
using Microsoft.Xna.Framework;

namespace MagicWorld.HelperClasses.Collision
{
    /// <summary>
    /// this class provides reusable functions for physics influeced behavior
    /// </summary>
    public class PhysicsManager
    {
        Level level;
        public PhysicsManager(Level level)
        {
            this.level = level;
        }

        /// <summary>
        /// Apply Gravity to an object; no collision handling
        /// </summary>
        /// <param name="elem">the game element</param>
        /// <param name="acceleration">the vector with which strength and direction the gravity goes</param>
        public virtual void ApplyGravity(BasicGameElement elem, Vector2 acceleration, GameTime time)
        {
            elem.Velocity += acceleration * (float)time.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Apply Gravity to an object; stops if collision occurs
        /// </summary>
        /// <param name="elem">the game element</param>
        /// <param name="acceleration">the vector with which strength and direction the gravity goes</param>
        /// <returns>True if has influence,false if not</returns>
        public virtual bool ApplyGravityWithCollisionDetection(BasicGameElement elem, Vector2 acceleration,GameTime time)
        {
            Vector2 oldPos = elem.Position;

            ApplyGravity(elem, acceleration, time);

            List<BasicGameElement> collisionObjects = new List<BasicGameElement>();
            level.CollisionManager.CollidateWithGeneralLevelElements(elem, ref collisionObjects);

            foreach (BasicGameElement t in collisionObjects)
            {
                if (typeof(Enemy) != t.GetType())
                {
                    CollisionType collision = t.Collision;
                    if (collision == CollisionType.Impassable
                        || (collision == CollisionType.Platform && t.Bounds.getRectangle().Bottom > elem.Bounds.getRectangle().Bottom))
                    {
                        Vector2 depth = CollisionManager.GetCollisionDepth(elem, t);

                        if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                        {
                            //increase distance until we have no more collision
                            if (depth.Y >= 0)
                            {
                                depth.Y++;
                            }
                            else
                            {
                                depth.Y--;
                            }
                            elem.Position += new Vector2(0, depth.Y);
                        }

                    }
                }
            }

            if (oldPos == elem.Position)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
