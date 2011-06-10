﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEffects;

namespace MagicWorld.DynamicLevelContent.ParticleEffects
{
    class ParticleSystemFactory
    {
        public static ParticleSystem getMagic(Level level,int howManyEffectsMax)
        {
            return new MagicParticleSystem(level, howManyEffectsMax);
        }

        public static ParticleSystem getExplosion(Level level, int howManyEffectsMax)
        {
            return new ExplosionParticleSystem(level, howManyEffectsMax);
        }

        public static ParticleSystem getExplosionSmoke(Level level, int howManyEffectsMax)
        {
            return new ExplosionSmokeParticleSystem(level, howManyEffectsMax);
        }

        public static ParticleSystem getSmoke(Level level, int howManyEffectsMax)
        {
            return new SmokePlumeParticleSystem(level, howManyEffectsMax);
        }

        public static ParticleSystem getMatterCreation(Level level, int howManyEffectsMax)
        {
            return new MatterCreationParticleSystem(level, howManyEffectsMax,100f);
        }
    }
}