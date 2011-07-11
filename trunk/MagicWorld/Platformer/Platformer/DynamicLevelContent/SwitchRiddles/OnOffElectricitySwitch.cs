﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MagicWorld.StaticLevelContent;
using Microsoft.Xna.Framework;
using MagicWorld.HelperClasses;
using ParticleEffects;
using MagicWorld.Audio;

namespace MagicWorld.DynamicLevelContent.SwitchRiddles
{
    /// <summary>
    /// this switch reacts on a electric spell and puts the switch on or off depending on state
    /// </summary>
    class OnOffElectricitySwitch : AbstractSwitch
    {
        int currentParticles = 0;

        public OnOffElectricitySwitch(String texture, Level level, Vector2 position, string id)
            : base(texture, CollisionType.Platform, level, position,id)
        {

            //calculate bounds
            int left = (int)Math.Round(position.X);
            int top = (int)Math.Round(position.Y);

            //the bounds are only a small rectangle at bottom middle of the texture
            int boundsWith = 30;
            int boundsHeight = 30;
            this.bounds = new Bounds(left + Width / 2 - boundsWith / 2, top + Height - boundsHeight, boundsWith, boundsHeight);

        }

        public override void Update(GameTime gameTime)
        {
            foreach (IActivation switchable in SwitchableObjects)
            {
                if (switchable.GetType() == typeof(SwitchableMoveablePlatform))
                {
                    SwitchableMoveablePlatform platform = (SwitchableMoveablePlatform)switchable;
                    if (platform.getMove())
                    {
                        currentParticles++;

                        if (currentParticles % 16 == 0) //only every .. update cycle
                        {
                            level.Game.LightningCreationParticleSystem.AddParticles(new ParticleSetting(position, 20));
                        }
                    }
                   // audioService.stopSoundLoop(SoundType.electric);
                } 
                else if (switchable.GetType() == typeof(MoveablePlatform))
                {
                    MoveablePlatform platform = (MoveablePlatform)switchable;
                    if (platform.isMoving)
                    {
                        currentParticles++;

                        if (currentParticles % 16 == 0) //only every .. update cycle
                        {
                            level.Game.LightningCreationParticleSystem.AddParticles(new ParticleSetting(position, 20));
                        }
                    }
                   // audioService.stopSoundLoop(SoundType.electric);
                }
            }
            base.Update(gameTime);
        }


        public override bool SpellInfluenceAction(Spell spell)
        {
            if (spell.GetType() == typeof(ElectricSpell))
            {
                if (Activated)
                {
                    Deactivate();
                }
                else
                {
                    Activate();
                }
                audioService.stopSoundLoop(SoundType.electric);
                return true;
            }
            return base.SpellInfluenceAction(spell);
        }
    }
}
