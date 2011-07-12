﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicWorld.Services;
using System.Diagnostics;

namespace MagicWorld.BlendInClasses
{
    public class TutorialManager : DrawableGameComponent
    {
        //TODO: Polishing: Improve Appearance of instructions.
        List<TutorialInstruction> instructions;
        SpriteFont font;
        SpriteBatch spriteBatch;
        IPlayerService playerService;
        Texture2D bg;
        Vector2 textOffset = new Vector2(20, 20);
        ICameraService cameraService = null;
        private Vector2 pos;

        public void AddInstruction(String text, Rectangle pos)
        {
            TutorialInstruction t = new TutorialInstruction(text, pos);
            t.Manager = this;
            instructions.Add(t);
        }

        public void AddInstructionSet(List<TutorialInstruction> instructs)
        {
            instructions.AddRange(instructs);
        }

        public TutorialManager(Game game)
            : base(game)
        {
            instructions = new List<TutorialInstruction>();
            playerService = (IPlayerService)Game.Services.GetService(typeof(IPlayerService));
        }

        public override void Initialize()
        {
            instructions = new List<TutorialInstruction>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Instructions/InstructionFont");
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            bg = Game.Content.Load<Texture2D>("Instructions/InstructionBG");
            cameraService = (ICameraService)Game.Services.GetService(typeof(ICameraService));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            pos = new Vector2(Game.GraphicsDevice.Viewport.Width / 2 - bg.Width / 2, 30);
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].IsActive)
                {
                    if (instructions[i].DisplayTime <= TimeSpan.Zero)
                        instructions.Remove(instructions[i]);//Check if the instruction has run for long enough
                    else
                    {
                        instructions[i].DisplayTime = instructions[i].DisplayTime.Subtract(gameTime.ElapsedGameTime);
                        instructions[i].Transparency = (float)(instructions[i].DisplayTime.TotalMilliseconds / instructions[i].InitialTime);
                    }
                }
                else
                {//Check if an instruction needs to be activated
                    if (instructions[i].Position.Contains((int)playerService.Position.X, (int)playerService.Position.Y))
                    {
                        // set all other instructions inactive
                        foreach (TutorialInstruction inst in instructions)
                        {
                            inst.IsActive = false;
                        }
                        //set new instructions active
                        instructions[i].IsActive = true;
                    }
                }
            }            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].IsActive)
                {
                    //instructions[i].Position;
                    //Debug.WriteLine("TutorialInstrucion pos: " + pos);//TEST
                    spriteBatch.Draw(bg, pos, Color.White * instructions[i].Transparency);
                    spriteBatch.DrawString(font, instructions[i].Text, pos + textOffset, Color.Black * instructions[i].Transparency);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
