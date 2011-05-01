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
    class CollisionManager
    {
        public void ObstacleCollisionResolution(List<GameObject> immovables, List<Hero> heroes)
        {
            List<GameObject> obstacle = new List<GameObject>();
            foreach (GameObject go in immovables)
            {
                foreach (Hero h in heroes)
                {
                    if (go.CollisionArea.Intersects(h.CollisionArea))//collision check
                    {
                        Vector2 correctionVector;
                        int x1 = Math.Abs(h.CollisionArea.Right - go.CollisionArea.Left);
                        int x2 = Math.Abs(h.CollisionArea.Left - go.CollisionArea.Right);
                        int y1 = Math.Abs(h.CollisionArea.Bottom - go.CollisionArea.Top);
                        int y2 = Math.Abs(h.CollisionArea.Top - go.CollisionArea.Bottom);

                        correctionVector.X = x1 < x2 ? x1 : x2;
                        correctionVector.Y = y1 < y2 ? y1 : y2;

                        h.Position += correctionVector;
                    }   
                }
            }
        }
    }
}
