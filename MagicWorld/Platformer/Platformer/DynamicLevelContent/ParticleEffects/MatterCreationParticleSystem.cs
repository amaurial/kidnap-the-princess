#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld;
using MagicWorld.HelperClasses;
#endregion

namespace ParticleEffects
{
    /// <summary>
    /// MatterCreationParticleSystem is a specialization of ParticleSystem which creates a
    /// imploding matter creation effect
    /// </summary>
    class MatterCreationParticleSystem : ParticleSystem
    {
        public MatterCreationParticleSystem(MagicWorldGame game, int howManyEffects)
            : base(game, howManyEffects)
        {
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "matterCreation";

            // high initial speed with lots of variance.  make the values closer
            // together to have more consistently circular explosions.
            minInitialSpeed = 1;
            maxInitialSpeed = 10;

            // doesn't matter what these values are set to, acceleration is tweaked in
            // the override of InitializeParticle.
            minAcceleration = 0;
            maxAcceleration = 0;

            // explosions should be relatively short lived
            minLifetime = .1f;
            maxLifetime = .3f;

            minScale = .005f;
            maxScale = .10f;

            // we need to reduce the number of particles on Windows Phone in order to keep
            // a good framerate

            minNumParticles = 5;
            maxNumParticles = 12;

            minRotationSpeed = -MathHelper.PiOver2;
            maxRotationSpeed = MathHelper.PiOver2;

            // additive blending is very good at creating fiery effects.
			blendState = BlendState.Additive;

            DrawOrder = AdditiveDrawOrder;
        }

        protected override void InitializeParticle(Particle p, ParticleSetting particleSetting)
        {
            base.InitializeParticle(p, particleSetting);

            p.Acceleration = p.Velocity*p.Lifetime*0.1f;
        }


        protected override Vector2 getStartPositionRelativeToCenter(ParticleSetting particleSetting)
        {
            return GeometryCalculationHelper.getRandomPositionOnCycleBow(particleSetting.position, particleSetting.distance);
        }

        protected override Vector2 PicDirection(ParticleSetting particleSetting, Vector2 startPosition)
        {
            return particleSetting.position - startPosition;
        }
    }
}
