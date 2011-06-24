﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleEffects;
using Microsoft.Xna.Framework;

namespace MagicWorld.DynamicLevelContent.ParticleEffects
{
    class ParticleSystemFactory
    {
        public static ParticleSystem getMagic(MagicWorldGame game,int howManyEffectsMax)
        {
            return new MagicParticleSystem(game, howManyEffectsMax);
        }

        public static ParticleSystem getIceMagic(MagicWorldGame game, int howManyEffectsMax)
        {
            return new MagicParticleSystem(game, howManyEffectsMax, Color.LightCyan);
        }

        public static ParticleSystem getFireMagic(MagicWorldGame game, int howManyEffectsMax)
        {
            return new MagicParticleSystem(game, howManyEffectsMax, Color.Red);
        }

        public static ParticleSystem getExplosion(MagicWorldGame game, int howManyEffectsMax)
        {
            return new ExplosionParticleSystem(game, howManyEffectsMax);
        }

        public static ParticleSystem getExplosionSmoke(MagicWorldGame game, int howManyEffectsMax)
        {
            return new ExplosionSmokeParticleSystem(game, howManyEffectsMax);
        }

        public static ParticleSystem getSmoke(MagicWorldGame game, int howManyEffectsMax)
        {
            return new SmokePlumeParticleSystem(game, howManyEffectsMax);
        }

        public static ParticleSystem getMatterCreation(MagicWorldGame game, int howManyEffectsMax)
        {
            return new MatterCreationParticleSystem(game, howManyEffectsMax,100f);
        }

        public static ParticleSystem getFire(MagicWorldGame game, int howManyEffectsMax)
        {
            return new FireParticleSystem(game, howManyEffectsMax);
        }
    }
}
