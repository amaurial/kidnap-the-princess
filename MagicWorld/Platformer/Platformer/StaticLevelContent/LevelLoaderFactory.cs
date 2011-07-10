﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicWorld.StaticLevelContent
{
    class LevelLoaderFactory
    {

        public const int NumberOfLevels = 9;
        /// <summary>
        /// Factory which gives you a new LevelLoader
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        public static ILevelLoader getLevel(int nr){
            //return new StaticLevelLoader(); //testing
            //return new XMLLevelLoader(5);//TEST
            if (nr <= NumberOfLevels) 
            {
                return new XMLLevelLoader(nr); 
            } 
            else 
            {
                return new XMLLevelLoader(1); 
            }
    }
    }
}
