﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MagicWorld.Constants
{
    class ConstantChangerItemInteger:IConstantChangerItem
    {

        public Int32 value;
        Int32 changeFactor;

        public string name;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of item</param>
        /// <param name="value">current value of item</param>
        /// <param name="changeFactor">should be a positive value</param>
        public ConstantChangerItemInteger(string name, Int32 value, Int32 changeFactor)
        {
            this.value = value;
            this.changeFactor=changeFactor;
            this.name = name;
        }

        #region IConstantChangerItem Member

        public void Increment()
        {
            value+=changeFactor;
        }

        public void Decrement()
        {
            value-=changeFactor;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Vector2 position, Microsoft.Xna.Framework.Graphics.SpriteFont font, Color color)
        {
            spriteBatch.DrawString(font, name +": " + value, position, color);
        }

        public void switchInternalValues()
        {
            //not used
        }

        public string Name()
        {
            return this.name;
        }

        #endregion
    }
}
